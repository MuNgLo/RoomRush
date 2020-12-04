using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DecaMovement.Base;
using DecaMovement.Events;

namespace DecaMovement
{
    public class MovementDebug : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            DecaMovementEvents.OnLand.AddListener(OnLandJump);
            DecaMovementEvents.OnJump.AddListener(OnLandJump);
        }

        private void OnLandJump(GroundTest groundData)
        {
            Debug.DrawLine(
                groundData.groundPoint,
                groundData.groundPoint + groundData.groundNormal * 0.6f,
                Color.magenta,
                3.0f
                );
        }


    }
}