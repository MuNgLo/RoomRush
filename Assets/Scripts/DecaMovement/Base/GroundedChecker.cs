using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecaMovement.Base
{
    /// <summary>
    /// This class should be on a GameObject under the DecaInput/Motor Object. It handles groundchecks and
    /// very likely needs its own layer in the physics settings.
    /// Make sure pivvot/origin of rigidbody is on surface we have towards cast direction.
    /// </summary>
    [AddComponentMenu("DoDecArena/Ground Checker"), RequireComponent(typeof(MeshFilter),typeof(MeshCollider),typeof(Rigidbody))]
    public class GroundedChecker : MonoBehaviour
    {
        public bool _debug = false;
        public bool _debugGrounded = false;
        public float _castDistance = 0.2f;
        private Rigidbody _rbGCheck = null;
        private Rigidbody _rbMotor = null;
        private GroundTest _groundData = new GroundTest();
        public GroundTest GroundData { get { return _groundData; } private set { } }
        private int _lastHitINstanceID = -1;
        private void Awake()
        {
            _rbGCheck = GetComponent<Rigidbody>();
            _rbMotor = transform.parent.GetComponent<Rigidbody>();
            _rbGCheck.constraints = RigidbodyConstraints.FreezeRotationZ;
            _rbGCheck.isKinematic = true;
            GetComponent<MeshCollider>().convex = true;
        }

        internal void ForceGroundLoss()
        {
            _groundData.groundPoint = Vector3.zero;
            _groundData.groundNormal = Vector3.zero;
            _groundData.distance = 0.0f;
            _groundData.grounded = false;
            _groundData.groundSpeed = Vector3.zero;
        }

        private void FixedUpdate()
        {
            // Skip check if we are moving away from ground
            if(Vector3.Dot(_rbMotor.velocity, -_groundData.groundNormal) > 0.0f)
            {
                if (_debug) { Debug.Log($"GroundChecker skipping!"); }
                ForceGroundLoss();
                return;
            }

            // Do a sweeptest with rigidbody and update the groundData accordingly.
            if (_rbGCheck.SweepTest(-transform.up, out RaycastHit hit, _castDistance))
            {
                if (_lastHitINstanceID != hit.collider.GetInstanceID())
                {
                    if (_debug) { Debug.Log($"GroundChecker found {hit.collider.name}"); }
                    _lastHitINstanceID = hit.collider.GetInstanceID();
                }
                _debugGrounded = true;
                _groundData.groundPoint = hit.point;
                _groundData.groundNormal = hit.normal;
                _groundData.distance = hit.distance;
                _groundData.grounded = true;
                if (hit.rigidbody)
                {
                    _groundData.groundSpeed = hit.rigidbody.velocity;
                }
                else
                {
                    _groundData.groundSpeed = Vector3.zero;
                }
            }
            else
            {
                if (_lastHitINstanceID != -1)
                {
                    if (_debug) { Debug.Log($"GroundChecker found nothing"); }
                    _lastHitINstanceID = -1;
                }
                _debugGrounded = false;
                _groundData.groundPoint = Vector3.zero;
                _groundData.groundNormal = Vector3.zero;
                _groundData.distance = 0.0f;
                _groundData.grounded = false;
                _groundData.groundSpeed = Vector3.zero;
            }
        }
        private void OnCollisionStay(Collision collision)
        {
            if (_debug) { Debug.Log($"GroundChecker collider overlapping {collision.collider.name}"); }
        }

    }// EOF Class
}