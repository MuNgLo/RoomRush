using DecaMovement.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DecaMovement.Moves
{
    // All jump related values and calculations here
    [System.Serializable]
    public class Jump
    {
        public float _jumpSpeed = 3.3f;          // The speed at which the character's up axis gains when hitting jump
        public float _coolDown = 0.3f;
        public float _slopeJumpInheritence = 0.75f;
        public bool CanJump { get { return JumpCheck(); } private set { } }
        public bool _jumped = false; // Flag this true when starting a jump. Reset to false when failing froundcheck.
        private float _lastJump = -10.0f;
        /// <summary>
        /// Returns true if currently able to Jump
        /// </summary>
        /// <returns></returns>
        private bool JumpCheck()
        {
            return Time.time > _lastJump + _coolDown;
        }
        /// <summary>
        /// Applies a Jump velocity vector to fallVelocity
        /// </summary>
        /// <param name="fallVelocity"></param>
        /// <param name="gravity"></param>
        /// <returns></returns>
        internal Vector3 DoJump(Rigidbody rb, Vector3 gravity, GroundTest groundData)
        {
            //Debug.Log($"JUMP! distance={groundData.distance}");
            // Make sure we start from same height always
            //rb.position = rb.position + rb.transform.up * (0.05f - groundData.distance);
            // Negate any downwards momentum before applying jump
            //if(Vector3.Dot(rb.velocity.normalized, gravity.normalized) > 0.0f)
            //{
                //rb.velocity = Vector3.ProjectOnPlane(rb.velocity, gravity);
            //}
            _lastJump = Time.time;
            // Apply jump
            //return rb.velocity + Vector3.Slerp(-gravity.normalized, groundData.groundNormal, _slopeJumpInheritence) * _jumpSpeed;
            Vector3 newVel = rb.velocity;
            newVel.y = 0.0f;
            return newVel + -gravity.normalized * _jumpSpeed;
        }
    }
}
