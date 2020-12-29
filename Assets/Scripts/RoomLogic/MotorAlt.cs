using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class MotorAlt : MonoBehaviour
{
    // TODO move a lot over to settings
    public float _jumpSpeed = 3.0f;
    public float _jumpCooldown = 0.3f;
    public float _friction = 1.0f;
    public float _ground_accelerate = 10.0f;
    public float _max_velocity_ground = 10.0f;
    public float _air_accelerate = 10.0f;
    public float _max_velocity_air = 10.0f;
    //private bool _teleported = false;
    private InputHandling _in = null;
    private CharacterController _cc = null;
    public bool IsGrounded { get { return _cc.isGrounded; } private set { } }
    [SerializeField]
    private bool _wasGrounded = true;
    [SerializeField]
    private float _tsJumpLast = 0.0f;
    private Vector3 _prevPosition = Vector3.zero;
    private Vector3 _prevVelocity = Vector3.zero;

    private void Awake()
    {
        _in = Core.Instance.Player.PlayerInput;
        _cc = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (_teleported) { _teleported = false; return; }
        _prevPosition = transform.position;
        Vector3 wishDir = new Vector3(_in.CMD.rightMove, 0.0f, _in.CMD.forwardMove);
        wishDir = transform.TransformDirection(wishDir).normalized;

        // Split velocity over gravity
        Vector3 flatVelocity = Vector3.zero;
        Vector3 verticalVelocity = Vector3.zero;
        if(_prevVelocity != Vector3.zero)
        {
            flatVelocity = Vector3.ProjectOnPlane(_prevVelocity, Vector3.down);
            verticalVelocity = Vector3.Project(_prevVelocity, Vector3.up);
        }

        if (!_cc.isGrounded)
        {
            if (wishDir != Vector3.zero)
            {
                flatVelocity = MoveAir(wishDir, flatVelocity);
            }
        }
        else
        {
            if (!_wasGrounded)
            {
                //Just landed so drop recidual vert speed
                verticalVelocity = Vector3.zero;
            }
            flatVelocity = MoveGround(wishDir, flatVelocity);
            if(_in.CMD.wishJump && Time.time > _tsJumpLast + _jumpCooldown)
            {
                verticalVelocity = Vector3.up * _jumpSpeed;
                _tsJumpLast = Time.time;
            }
        }
        // Anything below should run every frame
        verticalVelocity += Physics.gravity * Time.deltaTime;

        Vector3 frameMovement = verticalVelocity + flatVelocity;
        _cc.Move(frameMovement * Time.deltaTime);
        _prevVelocity = frameMovement;
        //_prevVelocity = (transform.position - _prevPosition) / Time.deltaTime;
        _wasGrounded = _cc.isGrounded;
    }

    internal void Teleport(Transform spawnPoint)
    {
        _cc.enabled = false;
        _prevPosition = spawnPoint.position;
        _prevVelocity = Vector3.zero;
        _tsJumpLast = Time.time - 10.0f;
        _wasGrounded = true;
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        //_teleported = true;
        _cc.enabled = true;

    }

    // accelDir: normalized direction that the player has requested to move (taking into account the movement keys and look direction)
    // prevVelocity: The current velocity of the player, before any additional calculations
    // accelerate: The server-defined player acceleration value
    // max_velocity: The server-defined maximum player velocity (this is not strictly adhered to due to strafejumping)
    private Vector3 Accelerate(Vector3 accelDir, Vector3 prevVelocity, float accelerate, float max_velocity)
    {
        float projVel = Vector3.Dot(prevVelocity, accelDir); // Vector projection of Current velocity onto accelDir.
        float accelVel = accelerate * Time.deltaTime; // Accelerated velocity in direction of movment

        // If necessary, truncate the accelerated velocity so the vector projection does not exceed max_velocity
        if (projVel + accelVel > max_velocity)
            accelVel = max_velocity - projVel;

        return prevVelocity + accelDir * accelVel;
    }

    private Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity)
    {
        // Apply Friction
        float speed = prevVelocity.magnitude;
        if (speed != 0) // To avoid divide by zero errors
        {
            float drop = speed * _friction * Time.deltaTime;
            prevVelocity *= Mathf.Max(speed - drop, 0) / speed; // Scale the velocity based on friction.
        }

        // ground_accelerate and max_velocity_ground are server-defined movement variables
        return Accelerate(accelDir, prevVelocity, _ground_accelerate, _max_velocity_ground);
    }

    private Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity)
    {
        // air_accelerate and max_velocity_air are server-defined movement variables
        return Accelerate(accelDir, prevVelocity, _air_accelerate, _max_velocity_air);
    }
}
