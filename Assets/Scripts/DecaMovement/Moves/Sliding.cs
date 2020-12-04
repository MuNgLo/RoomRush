using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DecaMovement.Moves
{
    [System.Serializable]
    public class Sliding
    {
        public bool _canSlide = false;
        /// <summary>
        /// Adds acceleration to flatVelocity. Amount relative to angle between groundNormal and gravity.
        /// </summary>
        /// <param name="flatVelocity"></param>
        /// <param name="groundNormal"></param>
        /// <param name="gravity"></param>
        /// <param name="delta"></param>
        internal void ApplySlide(ref Vector3 flatVelocity, Vector3 groundNormal, Vector3 gravity, float delta)
        {
            // Acceleration base. Max 1
            float dot = Mathf.Abs(Vector3.Dot(groundNormal, gravity.normalized));
            flatVelocity = Vector3.ProjectOnPlane(flatVelocity + -(gravity * dot * delta), groundNormal);
        }
        /// <summary>
        /// Changes widhDir to zero if the direction goes against gravity.
        /// </summary>
        /// <param name="wishDir"></param>
        /// <param name="gravity"></param>
        internal void AdjustInputVector(ref Vector3 wishDir, Vector3 groundNormal, Vector3 gravity)
        {
            Vector3 pullVector = Vector3.ProjectOnPlane(gravity, groundNormal).normalized;

            if(Vector3.Dot(wishDir, pullVector) < 0.0f)
            {
                wishDir = Vector3.ProjectOnPlane(wishDir, pullVector).normalized;
            }
        }
    }
}