using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DecaMovement.Base
{
    /// <summary>
    /// Holds all ground movement related values and calculates movement on ground.
    /// Note that friction is still in physics
    /// </summary>
    [System.Serializable]
    public class GroundMovement
    {
        public float moveSpeed = 8.0f;                // Ground move speed
        public float runAcceleration = 10.5f;         // Ground accel
        public float maxDegree = 45.0f;
        /// <summary>
        /// Takes current velocity, the direction the player wants to go and calculates a velocity change
        /// </summary>
        /// <param name="playerVelocity"></param>
        /// <param name="wishdir"></param>
        /// <returns></returns>
        internal Vector3 GroundMove(Vector3 playerVelocity, Vector3 wishdir)
        {
            var wishspeed = wishdir.magnitude;
            wishspeed *= moveSpeed;
            return Accelerate2(playerVelocity, wishdir, wishspeed, runAcceleration);
        }

        private Vector3 Accelerate2(Vector3 playerVelocity, Vector3 wishdir, float wishspeed, float accel)
        {
            float addspeed;
            float accelspeed;
            float currentspeed;

            currentspeed = Vector3.Dot(playerVelocity, wishdir);
            addspeed = wishspeed - currentspeed;
            if (addspeed <= 0)
                return playerVelocity;
            accelspeed = accel * Time.deltaTime * wishspeed;
            if (accelspeed > addspeed)
                accelspeed = addspeed;

            //playerVelocity.x += accelspeed * wishdir.x;
            //playerVelocity.z += accelspeed * wishdir.z;
            return playerVelocity + accelspeed * wishdir;
        }


        /// <summary>
        /// Calculates the acceleration tot speed up or slow down the movemnt.
        /// </summary>
        /// <param name="playerVelocity"></param>
        /// <param name="wishdir"></param>
        /// <param name="wishspeed"></param>
        /// <param name="accel"></param>
        /// <returns></returns>
        private Vector3 Accelerate(Vector3 playerVelocity, Vector3 wishdir, float wishspeed, float accel)
        {
            float addspeed;
            float accelspeed;
            float currentspeed;
            if (wishdir == Vector3.zero)
            {
                wishdir = -playerVelocity;
            }
            currentspeed = Vector3.Dot(playerVelocity, wishdir);
            addspeed = wishspeed - currentspeed;
            if (addspeed <= 0)
            {
                return playerVelocity;
            }
            accelspeed = accel * Time.deltaTime * wishspeed;
            if (accelspeed > addspeed)
                accelspeed = addspeed;
            playerVelocity += accelspeed * wishdir;
            return playerVelocity;
        }
    }// End of class
}
