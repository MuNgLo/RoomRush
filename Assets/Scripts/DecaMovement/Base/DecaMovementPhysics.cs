
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DecaMovement.Base
{
    /// <summary>
    /// This is the base physics for the movment. this handles cast to se if we can rotate player, move him or rotate.
    /// </summary>
    [System.Serializable]
    public class DecaMovementPhysics
    {
        public float runDeacceleration = 10.0f;       // Deacceleration that occurs when running on the ground
        public float friction = 6; //Ground friction
        //public float minGroundDistance = 0.1f;
        //public float maxGroundDistance = 0.2f;

        private float _lastGravityChange = 0.0f;
        [SerializeField]
        private Vector3 _currentGravity;
        public Vector3 Gravity { get { return _currentGravity; } set { SetGravity(value); } }

        /// <summary>
        /// Call this after initiazion to make additional preperations like setting an initial gravity
        /// </summary>
        internal void Setup()
        {
            _currentGravity = UnityEngine.Physics.gravity;
        }
        /// <summary>
        /// This makes sure we keep track of gravity vectors
        /// </summary>
        /// <param name="newGravity"></param>
        private void SetGravity(Vector3 newGravity)
        {
            if(newGravity == _currentGravity) { return; }
            _currentGravity = newGravity;
            _lastGravityChange = Time.time;
        }
        /// <summary>
        /// Applies friction to the player. Usually only when grounded
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="playerVelocity"></param>
        /// <returns></returns>
        internal Vector3 ApplyFriction(float modifier, Vector3 playerVelocity)
        {
            float speed = playerVelocity.magnitude;
            float drop = 0.0f;
            float control = speed < runDeacceleration ? runDeacceleration : speed;
            drop = control * friction * Time.deltaTime * modifier;
            float newspeed = speed - drop;
            if (newspeed < 0)
                newspeed = 0;
            if (speed > 0)
                newspeed /= speed;
            return playerVelocity * newspeed;
        }
        /// <summary>
        /// Checks distance to surface towards the min and max. If to far, applies gravity. If to close applies a repealing acceleration.
        /// When within tolerance of min/max it negates fallVelocity and returns a Vector3.Zero
        /// </summary>
        /// <param name="fallVelocity"></param>
        /// <param name="groundNormal"></param>
        /// <param name="currentDistance"></param>
        /*internal void StickToSurface(ref Vector3 fallVelocity, Vector3 groundNormal, float currentDistance)
        {
            if (currentDistance > maxGroundDistance)
            {
                fallVelocity += Gravity * Time.deltaTime;
            }
            else if (currentDistance < minGroundDistance)
            {
                fallVelocity += groundNormal * 1.0f * Time.deltaTime;
            }else
            {
                fallVelocity = Vector3.zero;
            }
        }*/
    }// End of class
}
