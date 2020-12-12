using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RoomLogic;
using System;

namespace Enemies
{
    public class EnemyAI : RoomObjectBehaviour
    {
        public int _updateRate = 10;


        private int _nextUpdateIn = 0;
        private GameObject _target = null;
        private EnemyState _eState = null;
        private ENEMYSTATE State { get => _eState.State; set { } }

        public GameObject Target { get => _target; private set => _target = value; }

        private NavMeshAgent _navAgent = null;
        private ConditionBehaviour _condition = null;
        private RoomDriver _room = null;

        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            _room = roomDriver;
            _condition = conditionScript;
            _eState = GetComponent<EnemyState>();
            _navAgent = GetComponent<NavMeshAgent>();
            _room.OnRoomUpdate.AddListener(RoomUpdate);
            Core.Instance.Rooms.OnRoomActivated.AddListener(OnRooMActivated);

        }
        private void OnRooMActivated(GameObject playerAvatar)
        {
            _target = playerAvatar;
        }
        public override void RoomUpdate(float roomDeltaTime)
        {
            if(State == ENEMYSTATE.DEAD || State == ENEMYSTATE.INACTIVE)
            {
                // We ded Don't do shit
                return;
            }
            _nextUpdateIn--;
            if(_nextUpdateIn < 1)
            {
                _nextUpdateIn = _updateRate;
                float targetDistance = Vector3.Distance(Target.transform.position, transform.position);
                
                if (targetDistance < Core.Instance.Settings.Enemies.ReactDistance)
                {
                    if (targetDistance < Core.Instance.Settings.Enemies.ReactDistance)
                    {
                        // Avoid moving to close to player
                        if (targetDistance < Core.Instance.Settings.Enemies.MeleeReach - 0.3)
                        {
                            _navAgent.isStopped = true;
                        }
                        else
                        {
                            // Target Close but not to close so move towards target
                            MoveTowardsTarget();
                        }
                        if (targetDistance < Core.Instance.Settings.Enemies.MeleeReach)
                        {
                            DoMeleeAttack();
                            return;
                        }
                        return;
                    }
                }
            }
        }

        private void MoveTowardsTarget()
        {
            _navAgent.isStopped = false;
            _eState.ChangeState(ENEMYSTATE.MOVING);
            _navAgent.destination = Target.transform.position;
        }

        private void DoMeleeAttack()
        {
            //Debug.Log("MELEE WOP WOP!");
        }
    }// EOF CLASS
}