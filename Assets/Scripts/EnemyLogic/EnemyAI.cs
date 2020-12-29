using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RoomLogic;
using System;

namespace Enemies
{
    [RequireComponent(typeof(EnemyAction))]
    public class EnemyAI : RoomObjectBehaviour
    {

        private int _nextUpdateIn = 0;
        public Transform _head = null;
        private GameObject _target = null;
        private ENEMYSTATE State { get => _eState.State; set { } }

        public GameObject Target { get => _target; private set => _target = value; }
        

        private EnemyState _eState = null;
        private NavMeshAgent _navAgent = null;
        private EnemyAction _actions = null;
        private ConditionBehaviour _condition = null;
        private RoomDriver _room = null;

        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            _room = roomDriver;
            _condition = conditionScript;
            _eState = GetComponent<EnemyState>();
            _navAgent = GetComponent<NavMeshAgent>();
            _actions = GetComponent<EnemyAction>();
            _room.OnRoomUpdate.AddListener(RoomUpdate);
            Core.Instance.Rooms.OnRoomActivated.AddListener(OnRooMActivated);

        }
        private void OnRooMActivated(GameObject playerAvatar)
        {
            _target = playerAvatar;
        }

        public override void RoomUpdate(float roomDeltaTime)
        {
            if (State == ENEMYSTATE.DEAD || State == ENEMYSTATE.INACTIVE || State == ENEMYSTATE.STUNNED)
            {
                // We ded Don't do shit
                return;
            }
            if (_target)
            {
                _head.LookAt(_target.transform);
            }
            // Check if AI needs update
            _nextUpdateIn--;
            if (_nextUpdateIn < 1)
            {
                _nextUpdateIn = Core.Instance.Settings.Enemies.AIUpdateRate;
                // Run AI Update
                // If we are close enough we should just mellee 
                if (Core.Instance.Settings.Enemies.MeleeReach > Vector3.Distance(Target.transform.position, transform.position))
                {
                    _eState.ChangeState(ENEMYSTATE.MELEE);
                    return;
                }
                // Check if player is looking
                bool isPlayerLooking = Vector3.Dot(Target.transform.forward, transform.position - Target.transform.position) > 0.1f;

                if (isPlayerLooking)
                {
                    // Player is looking so we stalk
                    _eState.ChangeState(ENEMYSTATE.STALKING);
                }
                else
                {
                    // Player is not looking so we rush them
                    _eState.ChangeState(ENEMYSTATE.RUSHING);
                }
            }
        }

        
    }// EOF CLASS
}