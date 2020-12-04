using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DecaMovement.Base
{
    /// <summary>
    /// Holds all air movement related values and calculates movement in air.
    /// </summary>
    [System.Serializable]
    public class AirMovement
    {
        public float moveSpeed = 8.0f;                // Air move speed
        public float airAcceleration = 1.2f;          // Air accel
        public float airBrakeAcceleration = 5.0f;         // Deacceleration experienced when oposite 
        public float airControl = 0.14f;               // How precise air control is
        public float strafeOnlyAirControl = 0.14f;      // How precise air control is when only using strafe
        public float strafeOnlyAirAcceleration = 0.0f;  // Air accel when only sideStrafe
        public float strafeOnlyMaxSpeed = 1.5f;          // What the max speed to generate when side strafing
        internal Vector3 AirMove(Vector3 velocity, Vector3 wishdir, Cmd cmd)
        {
            // Sort out which acceleration and maxspeed to use
            float accel = airAcceleration;
            float wishspeed = moveSpeed;
            float tickControl = airControl;
            if (Vector3.Dot(velocity, wishdir) < 0)
            {
                accel = airBrakeAcceleration;
            }else if (cmd.forwardMove == 0 && cmd.rightMove != 0)
            {
                tickControl = strafeOnlyAirAcceleration;
                wishspeed = strafeOnlyMaxSpeed;
            }
            

            velocity = Accelerate(velocity, wishdir, wishspeed, accel);
            velocity = AirControl(velocity, wishdir, cmd, wishspeed, tickControl);


            return velocity;
        }
        internal Vector3 AirMove2(Vector3 playerVelocity, Vector3 wishdir, Cmd cmd)
        {
            float wishvel = airAcceleration;
            float accel;
            float control = airControl;

            float wishspeed = wishdir.magnitude;
            wishspeed *= moveSpeed;

            wishdir.Normalize();

            //float wishspeed2 = wishspeed;
            if (Vector3.Dot(playerVelocity, wishdir) < 0)
                accel = airBrakeAcceleration;
            else
                accel = airAcceleration;
            // If the player is ONLY strafing left or right
            if (cmd.forwardMove == 0 && cmd.rightMove != 0)
            {
                if (wishspeed > strafeOnlyMaxSpeed)
                    wishspeed = strafeOnlyMaxSpeed;
                accel = strafeOnlyAirAcceleration;
                control = strafeOnlyAirControl;
            }

            Accelerate2(ref playerVelocity, wishdir, wishspeed, accel);
            //if (airControl > 0)
            AirControl2(ref playerVelocity, wishdir, wishspeed, cmd, control);
            // !CPM: Aircontrol

            return playerVelocity;
        }
        private void Accelerate2(ref Vector3 playerVelocity, Vector3 wishdir, float wishspeed, float accel)
        {
            float addspeed;
            float accelspeed;
            float currentspeed;

            currentspeed = Vector3.Dot(playerVelocity.normalized, wishdir);
            addspeed = wishspeed - currentspeed;
            if (addspeed <= 0)
                return;
            accelspeed = accel * Time.deltaTime * wishspeed;
            if (accelspeed > addspeed)
                accelspeed = addspeed;

            playerVelocity += accelspeed * wishdir;
            //playerVelocity += accelspeed * Vector3.Project(wishdir, playerVelocity);
        }

        private void AirControl2(ref Vector3 playerVelocity, Vector3 wishdir, float wishspeed, Cmd cmd, float control)
        {
            float speed = playerVelocity.magnitude;
            //float dot = (Vector3.Dot(playerVelocity.normalized, wishdir) + 1.0f) * 0.5f;
            float dot = Vector3.Dot(playerVelocity.normalized, wishdir);
            Vector3 newDir = playerVelocity + wishdir * Math.Abs(dot) * control;
                newDir.Normalize();
                playerVelocity = newDir * speed;

        }

        /// <summary>
        /// Calculates the air acceleration.
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="wishdir"></param>
        /// <param name="wishspeed"></param>
        /// <param name="accel"></param>
        /// <returns></returns>
        private Vector3 Accelerate(Vector3 velocity, Vector3 wishdir, float wishspeed, float accel)
        {
            float addspeed;
            float accelspeed;
            float currentspeed;

            currentspeed = Vector3.Dot(velocity, wishdir);

            addspeed = wishspeed * currentspeed;
            if (addspeed <= 0)
            {
                return velocity;
            }
            accelspeed = accel * Time.deltaTime * wishspeed;
            if (accelspeed > addspeed)
                accelspeed = addspeed;

            return velocity + accelspeed * wishdir;
        }
        /// <summary>
        /// Air control occurs when the player is in the air, it allows players to move side to side much faster rather than being
        /// 'sluggish' when it comes to cornering.
        /// </summary>
        /// <param name="flatVelocity"></param>
        /// <param name="wishdir"></param>
        /// <param name="cmd"></param>
        /// <param name="wishspeed"></param>
        /// <returns></returns>
        private Vector3 AirControl(Vector3 flatVelocity, Vector3 wishdir, Cmd cmd, float wishspeed, float control)
        {
            // Can't control movement if not moving forward or backward
            if (Mathf.Abs(cmd.forwardMove) < 0.001 || Mathf.Abs(wishspeed) < 0.001) { 
                return flatVelocity;
                }
            float speed = flatVelocity.magnitude;
            flatVelocity.Normalize();
            float dot = Vector3.Dot(flatVelocity, wishdir);
            float k = control * dot * dot * Time.deltaTime;

            flatVelocity.x = flatVelocity.x * speed + wishdir.x * k;
            flatVelocity.y = flatVelocity.y * speed + wishdir.y * k;
            flatVelocity.z = flatVelocity.z * speed + wishdir.z * k;
            flatVelocity.Normalize();
            return flatVelocity * speed;
        }
    }// end of class
}
