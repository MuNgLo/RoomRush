using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DecaMovement.Base
{
    [AddComponentMenu("DoDecArena/Head Checker"), RequireComponent(typeof(MeshCollider), typeof(Rigidbody))]

    public class HeadChecker : MonoBehaviour
    {
        public bool _debug = false;

        public float _castDistance = 0.8f;
        private Rigidbody _rb = null;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            _rb.isKinematic = true;
            GetComponent<MeshCollider>().convex = true;
        }
        /// <summary>
        /// Check if head is blocked. Returns true if not blocked.
        /// </summary>
        /// <param name="crouched"></param>
        /// <returns></returns>
        internal bool HeadCheck(bool crouched)
        {
            // Do a sweeptest with rigidbody and return true if we don't hit anything
            if (_rb.SweepTest(transform.up, out RaycastHit hit, _castDistance))
            {
                if (_debug) { Debug.Log($"HeadChecker found {hit.collider.name}"); }
                return false;
            }
            else
            {
                if (_debug) { Debug.Log($"HeadChecker found nothing"); }
                return true;
            }
        }
        private void OnCollisionStay(Collision collision)
        {
            if (_debug) { Debug.Log($"GroundChecker colider overlapping {collision.collider.name}"); }
        }
    }
}