using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RoomLogic;

namespace Enemies
{
    public class RagDollControls : RoomObjectBehaviour
    {
        
        // MAKE DAMN SURE FIRST ENTRY IS HIP!!!!
        public Rigidbody[] _hitBoxes = null;
        private Animator _anims = null;

        private Vector3 _hipVelocity = Vector3.zero;
        private EnemyState _eState = null;
        private NavMeshAgent  _navAgent = null;

        private void Awake()
        {
            _eState = GetComponent<EnemyState>();
            _navAgent = GetComponent<NavMeshAgent>();
            _anims = GetComponent<Animator>();
        }

        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
        }

        public override void RoomUpdate(float roomDeltaTime)
        {
            float frameSpeed = Core.Instance.Settings.Enemies.RavMoveSpeed * Core.Instance.Rooms.RoomTimeMultiplier;
            _navAgent.speed = frameSpeed;
            if (_eState.CurrentMoveSpeed > 0.01f)
            {
                _anims.SetFloat("MoveSpeed", frameSpeed / _eState.CurrentMoveSpeed);
            }
            else
            {
                _anims.SetFloat("MoveSpeed", 0.0f);
            }
            if(_eState.State == ENEMYSTATE.STUNNED)
            {
                transform.Translate(_hipVelocity * roomDeltaTime, Space.World);
                _hipVelocity -= _hipVelocity * Core.Instance.Settings.Enemies.RagDrag * roomDeltaTime;
                //_hipVelocity = Vector3.MoveTowards(_hipVelocity, Vector3.zero, roomDeltaTime);
            }
            
        }

        internal void MakeAllKinematic()
        {
            foreach (Rigidbody rb in _hitBoxes)
            {
                rb.isKinematic = true;
            }
            _anims.enabled = true;
        }
        /// <summary>
        /// makes all hitboxes physics driven
        /// feed skiphip true if hip should stay kinematic
        /// </summary>
        /// <param name="skipHip"></param>
        internal void MakeAllNonKinematic(bool skipHip = false)
        {
            foreach (Rigidbody rb in _hitBoxes)
            {
                rb.isKinematic = false;
            }
            if (skipHip)
            {
                _hitBoxes[0].isKinematic = true;
            }
            _anims.enabled = false;
        }
        internal void PushHip(Vector3 pushVelocity)
        {
            _hipVelocity = Vector3.ClampMagnitude(_hipVelocity + pushVelocity * Core.Instance.Settings.Enemies.RagPushModifier, Core.Instance.Settings.Enemies.RagMaxSpeed);
        }
    }// EOF CLASS
}