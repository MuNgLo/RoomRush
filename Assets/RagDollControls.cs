using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomLogic;

namespace Enemies
{
    public class RagDollControls : RoomObjectBehaviour
    {
        
        // MAKE DAMN SURE FIRST ENTRY IS HIP!!!!
        public Rigidbody[] _hitBoxes = null;


        private Vector3 _hipVelocity = Vector3.zero;
        private EnemyState _eState = null;

        private void Awake()
        {
            _eState = GetComponent<EnemyState>();
        }

        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
        }

        public override void RoomUpdate(float roomDeltaTime)
        {
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
        }
        internal void MakeAllNonKinematic(bool skipHip = true)
        {
            foreach (Rigidbody rb in _hitBoxes)
            {
                rb.isKinematic = false;
            }
            if (!skipHip) { _hitBoxes[0].isKinematic = true; }
        }
        internal void PushHip(Vector3 pushVelocity)
        {
            _hipVelocity = Vector3.ClampMagnitude(_hipVelocity + pushVelocity * Core.Instance.Settings.Enemies.RagPushModifier, Core.Instance.Settings.Enemies.RagMaxSpeed);
        }
    }// EOF CLASS
}