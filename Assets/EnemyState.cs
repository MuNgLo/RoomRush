using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RoomLogic;
using System;

namespace Enemies {
    public enum ENEMYSTATE { INACTIVE, IDLE, MOVING, STUNNED, DEAD }

    [RequireComponent(typeof(NavMeshAgent), typeof(NavMeshIntegration), typeof(RagDollControls))]

    public class EnemyState : RoomObjectBehaviour
    {
        [SerializeField]
        private ENEMYSTATE _state = ENEMYSTATE.INACTIVE;

        public ENEMYSTATE State { get => _state; set { ChangeState(value); } }


        private NavMeshAgent _navAgent;
        private NavMeshIntegration _navIntegration;
        private RagDollControls _ragControls;

        internal void TakeHit(Vector3 force)
        {
            if (Core.Instance.Rooms.CurrentRoomState == ROOMSTATE.ACTIVE)
            {
                ChangeState(ENEMYSTATE.STUNNED);
                _ragControls.PushHip(force);
                if (State != ENEMYSTATE.STUNNED)
                {
                    StartCoroutine("StunTimer");
                }
            }
        }
        IEnumerator StunTimer()
        {
            yield return new WaitForSeconds(Core.Instance.Settings.Enemies.StunTime);
            if (Core.Instance.Rooms.CurrentRoomState == ROOMSTATE.ACTIVE)
            {
                ChangeState(ENEMYSTATE.DEAD);
            }
        }

        private void ChangeState(ENEMYSTATE newState)
        {
            _state = newState;
            switch (newState)
            {
                case ENEMYSTATE.INACTIVE:
                    break;
                case ENEMYSTATE.IDLE:
                    _ragControls.MakeAllKinematic();
                    _navAgent.enabled = true;
                    break;
                case ENEMYSTATE.MOVING:
                    _ragControls.MakeAllKinematic();
                    _navAgent.enabled = true;
                    break;
                case ENEMYSTATE.STUNNED:
                    _ragControls.MakeAllNonKinematic();
                    _navAgent.enabled = false;
                    break;
                case ENEMYSTATE.DEAD:
                    _ragControls.MakeAllNonKinematic(false);
                    _navAgent.enabled = false;
                    break;
                default:
                    break;
            }
        }

        public override void RoomUpdate(float roomDeltaTime)
        {

        }

        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _navIntegration = GetComponent<NavMeshIntegration>();
            _ragControls = GetComponent<RagDollControls>();
            _navAgent.enabled = true;
            _navIntegration.enabled = false;
            _ragControls.MakeAllKinematic();
            _state = ENEMYSTATE.INACTIVE;

            roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
            Core.Instance.Rooms.OnRoomActivated.AddListener(OnRoomActivated);
        }

        private void OnRoomActivated(GameObject playerAvatar)
        {
            ChangeState(ENEMYSTATE.IDLE);
        }
    }// EOF CLASS
}