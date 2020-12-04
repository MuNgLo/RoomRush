using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;

namespace DecaMovement.Base
{
    [Serializable]
    public struct GroundTest
    {
        public bool grounded;
        public Vector3 groundPoint;
        public Vector3 groundNormal;
        public Vector3 groundSpeed;
        public float distance;
    }
    // Contains the command the user wishes upon the character
    [Serializable]
    public struct Cmd
    {
        public bool airRotation;
        public float forwardMove;
        public float rightMove;
        public bool wishJump;
        public bool crouch;
    }
    [Serializable]
    public struct PlayerMovementEventArguments
    {
        public bool InputActive;
        public bool CanJump;
        public bool MovementActive;
        /// <summary>
        /// Set this true if you want rigidbody simulated.
        /// </summary>
        public bool Simulate;
    }
    [Serializable]
    public struct PlayerMovementResetArguments
    {
        public Vector3 Location;
        public Quaternion Rotation;
        public Vector3 Gravity;
    }
    [Serializable]
    public class PlayerJumpEvent : UnityEvent<GroundTest>
    {
    }
    [Serializable]
    public class PlayerLandEvent : UnityEvent<GroundTest>
    {
    }
    [Serializable]
    public class PlayerMovementEvent : UnityEvent<PlayerMovementEventArguments>
    {
    }
    [Serializable]
    public class PlayerMovementResetEvent : UnityEvent<PlayerMovementResetArguments>
    {
    }
}// EOF Namespace
