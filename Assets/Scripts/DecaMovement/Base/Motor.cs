using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecaMovement.Base
{
    /// <summary>
    /// The component calss holding all the movement related code.
    /// NOTE The MeshCollider has to be simple and flagged as convex.
    /// </summary>
    [AddComponentMenu("DoDecArena/Add Player Movement"), RequireComponent(typeof(Rigidbody), typeof(MeshCollider))]
    public class Motor : MonoBehaviour
    {
        //public Transform playerView;     // Camera
        

        public bool _debug = true;
        [Header("Settings")]
        public float _rotationSpeed = 120.0f; // How fast we rotate towards gravity when doing it manually
        public float _rotationSpeedModifier = 0.3f; // modifier to rotationSpeed when we rotate without manual input
        //public float CurrentSpeed { get => _currentSpeed * 10.0f; private set { } }
        [Header("References")]
        public GroundedChecker _groundCheck = null; // Don't forget this in the inspector
        public HeadChecker _headCheck = null; // Don't forget this in the inspector
        //public Transform _sidePivvot = null; // The child object that we rotate left/right and translate the inputvector
        [Header("Modules")]
        public DecaMovementPhysics _Physics = new DecaMovementPhysics();
        public GroundMovement _GroundMovement = new GroundMovement();
        public AirMovement _AirMovement = new AirMovement();
        [Header("Moves")]
        public Moves.Jump _Jump = new Moves.Jump();
        public Moves.Sliding _Sliding = new Moves.Sliding();
        #region private fields and properties
        //private float _currentSpeed = 0.0f;
        private bool _wasGrounded = true; // remember the groudned flag from last tic
        private bool _isCrouched = false;
        private Rigidbody _rb = null;
        private Vector3 _focusPoint = Vector3.zero;
        public bool IsGrounded { get { return _groundCheck.GroundData.grounded; } private set { } }
        public Vector3 Velocity { get { return _rb.velocity; } private set { } }
        public Vector3 Gravity { get { return _Physics.Gravity; } private set { } }

        private InputHandling _in =null;
        private Vector3 _outerForcesForFrame = Vector3.zero; // This stacks forces from none movement code to be applied at end of movement. Like moving platform and jumppad

        #endregion


        void Awake()
        {
            Events.DecaMovementEvents.OnPlayerMovementEvent.AddListener(OnPlayerMovementEvent);
            Events.DecaMovementEvents.OnPlayerMovementResetEvent.AddListener(OnPlayerMovementReset);
            _rb = GetComponent<Rigidbody>();
            _Physics.Setup();
            _rb.useGravity = false;
            //this.enabled = false;
            _in = GetComponent<InputHandling>();
        }
        // TODO needs work
        private void OnPlayerMovementEvent(PlayerMovementEventArguments args)
        {
            //Debug.Log($"Motor::OnPlayerMovementEvent() {args}");
            this.enabled = args.MovementActive;
            if (!args.Simulate) { Freeze(); }
        }
        private void OnPlayerMovementReset(PlayerMovementResetArguments args)
        {
            transform.localPosition = args.Location;
            transform.localRotation = args.Rotation;
            _rb.velocity = Vector3.zero;
            SetGravity(Physics.gravity.normalized, 1.0f);
        }

        private void FixedUpdate()
        {
            if (!IsGrounded)
            {
                AirMovement();
                return;
            }

            /*if (!_wasGrounded && _Jump.CanJump && _in.CMD.wishJump)
            {
                //PushAway();
                //AirMovement();
                GroundMovement();
                _rb.velocity = _Jump.DoJump(_rb, _Physics.Gravity, _groundCheck.GroundData);
                Events.DecaMovementEvents.RaiseOnJumpEvent(_groundCheck.GroundData);
                return;
            }*/
    
            //PushAway();
            GroundMovement();
            _wasGrounded = true;
            if (_Jump.CanJump && _in.CMD.wishJump)
            {
                _rb.velocity = _Jump.DoJump(_rb, _Physics.Gravity, _groundCheck.GroundData);
                Events.DecaMovementEvents.RaiseOnJumpEvent(_groundCheck.GroundData);
            }
        }



        private void GroundMovement()
        {
            // Break apart velocity over ground surface
            Vector3 flatVelocity = Vector3.ProjectOnPlane(_rb.velocity, _groundCheck.GroundData.groundNormal);
            // This is the current movement input vector projected on gravity relative to player forward direction
            Vector3 wishDir = Vector3.ProjectOnPlane(
                transform.TransformDirection(new Vector3(_in.CMD.rightMove, 0, _in.CMD.forwardMove)),
                _Physics.Gravity.normalized).normalized;

            // Detect steep surface
            /*if (_in.CMD.crouch && _currentSpeed > _GroundMovement.moveSpeed + 1.0f || Vector3.Angle(_groundCheck.GroundData.groundNormal, -_Physics.Gravity) > _GroundMovement.maxDegree)
            {
                Debug.Log("STEEP!");
                _Sliding.AdjustInputVector(ref wishDir, _groundCheck.GroundData.groundNormal, _Physics.Gravity); // Würkz!
                _Sliding.ApplySlide(ref flatVelocity, _groundCheck.GroundData.groundNormal, -_Physics.Gravity, Time.deltaTime);
            }*/
            flatVelocity = _Physics.ApplyFriction(1.0f, flatVelocity);
            flatVelocity = _GroundMovement.GroundMove(Vector3.ProjectOnPlane(flatVelocity, _groundCheck.GroundData.groundNormal), wishDir);
            // Apply velocity changes (fallvelocity has jump speed)
            _rb.velocity = Vector3.ProjectOnPlane(flatVelocity , _groundCheck.GroundData.groundNormal);
        }

        private void AirMovement()
        {
            // Break apart velocity over gravity vector
            Vector3 flatVelocity = Vector3.ProjectOnPlane(_rb.velocity, _Physics.Gravity);
            Vector3 fallVelocity = _rb.velocity - flatVelocity;
            // Prepare Wish direction vector
            //Vector3 wishDir = Vector3.ProjectOnPlane(transform.TransformDirection(new Vector3(_in.CMD.rightMove, 0, _in.CMD.forwardMove)), _Physics.Gravity).normalized;
            Vector3 wishDir = Vector3.ProjectOnPlane(transform.TransformDirection(new Vector3(_in.CMD.rightMove, 0, _in.CMD.forwardMove)), _Physics.Gravity);
            if (_Jump._jumped) { _Jump._jumped = false; }
            // In air we adjust the flat velocity to let gravity build over frames in the fall velocity
            flatVelocity = _AirMovement.AirMove2(flatVelocity, wishDir, _in.CMD);
            // Apply gravity
            ApplyGravity(ref fallVelocity);
            // Apply velocity changes
            _rb.velocity = flatVelocity + fallVelocity;
            _wasGrounded = false;
        }

        /// <summary>
        /// This instantly freezes the rigidbody where it is by setting its velocity to 0
        /// </summary>
        internal void Freeze()
        {
            _rb.velocity = Vector3.zero;
        }

        private void ApplyGravity(ref Vector3 vVel)
        {
            vVel += _Physics.Gravity * Time.deltaTime;
        }
        internal void SetGravity(Vector3 newGravity, float g)
        {
            _Physics.Gravity = newGravity.normalized * 9.81f * g;
        }
        /// <summary>
        /// Rotates rigidBody towards gravity using rotationSpeed.
        /// If doAirRotation is false rotationSpeed is multiplied with rotationSpeedModifier.
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="doAirRotation"></param>
        /*internal void GravityCompliance(float delta, bool doAirRotation)
        {
           if(_focusPoint == Vector3.zero)
            {
                // Keep view on focuspoint while rotating
                _focusPoint = _in.playerView.position + _in.playerView.forward * 30.0f;
            }
            
            float rotationSpeed = doAirRotation ? _rotationSpeed : _rotationSpeed * _rotationSpeedModifier;

            Vector3 rotAxis = Vector3.ProjectOnPlane(_rb.rotation * Vector3.forward, _Physics.Gravity).normalized;
            if(rotAxis == Vector3.zero)
            {
                Debug.LogWarning("VECTOR ZERO ROTATION COOKIE SAYS THIS BE BAD!!!");
                rotAxis = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(rotAxis, -_Physics.Gravity);
            Quaternion rotationTweak = Quaternion.RotateTowards(
                _rb.rotation,
                targetRotation,
                rotationSpeed * delta
                );
            _rb.rotation = rotationTweak;

            // Horizontal first
            Vector3 focusDirection = Vector3.ProjectOnPlane(_focusPoint - _rb.position, _rb.rotation * Vector3.up);
            if (Vector3.Angle(focusDirection, _rb.rotation * Vector3.forward) > 90.0f)
            {
                focusDirection = -focusDirection;
            }
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, Quaternion.LookRotation(focusDirection, _rb.rotation * Vector3.up), 30.0f * Time.fixedDeltaTime);

        }*/

        /*internal void ApplyJumppadForce(Vector3 direction, float force)
        {
            Vector3 jpRelativeVerticalSpeed = Vector3.Project(_rb.velocity, direction);
            // Check if we have to compensate when player is moving opposite to jumppad launch direction
            // Makes for consistent launches
            if (Vector3.Dot(direction, jpRelativeVerticalSpeed) < 0)
            {
                _outerForcesForFrame += direction * force - jpRelativeVerticalSpeed;
            }
            else
            {
                _outerForcesForFrame += direction * force;
            }





            //_groundCheck.ForceGroundLoss();
        }
        internal void ApplyOutsideExplosionForce(Vector3 direction, float force)
        {
            _rb.velocity += direction * force;
        }*/

        /// <summary>
        /// Dampens player velocity by percent over second
        /// </summary>
        /// <param name="v">1-100</param>
        internal void Dampen(int percent, float delta, bool noGrav = false)
        {
            percent = Mathf.Clamp(percent, 0, 1000);
            //_playerVelocity *= 1.0f - percent * delta;
            _rb.velocity -= _rb.velocity * ((percent / 100.0f) * delta);
            if (noGrav)
            {
                _rb.velocity += -_Physics.Gravity * delta;
            }
        }

        

    }// End of Class
}