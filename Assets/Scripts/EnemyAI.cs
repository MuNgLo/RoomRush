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

        private float _tsLastMelee = 100.0f;
        private int _nextUpdateIn = 0;
        private GameObject _target = null;
        private EnemyState _eState = null;
        private ENEMYSTATE State { get => _eState.State; set { } }

        public GameObject Target { get => _target; private set => _target = value; }
        public Transform _meleePoint1;
        public Transform _meleePoint2;

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
            _tsLastMelee = Core.Instance.Runs.CurrentTotalTime + 10.0f;
            Core.Instance.Rooms.OnRoomActivated.AddListener(OnRooMActivated);

        }
        private void OnRooMActivated(GameObject playerAvatar)
        {
            _target = playerAvatar;
        }
        public override void RoomUpdate(float roomDeltaTime)
        {
            if (State == ENEMYSTATE.DEAD || State == ENEMYSTATE.INACTIVE)
            {
                // We ded Don't do shit
                return;
            }
            _nextUpdateIn--;
            if (_nextUpdateIn < 1)
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
                            if (Core.Instance.Runs.CurrentTotalTime < _tsLastMelee - Core.Instance.Settings.Enemies.RavMeleeCooldown)
                            {
                                DoMeleeAttack();
                            }
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
            RaycastHit[] hits = Physics.CapsuleCastAll(_meleePoint1.position, _meleePoint2.position, Core.Instance.Settings.Enemies.RavMeleeRadius, transform.forward, 0.1f);
            Debug.Log($"MeleeAttack! {hits.Length} colliders hit");
            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.name == name) { return; }
                if (hit.collider.GetComponent<CharacterController>())
                {
                    Debug.Log($"MeleeAttack! Hit Player!");
                    _tsLastMelee = Core.Instance.Runs.CurrentTotalTime - Core.Instance.Settings.Enemies.RavMeleePenalty;
                    Core.Instance.Runs.ForcedPenaltyTime(Core.Instance.Settings.Enemies.RavMeleePenalty, true);
                }
            }
        }
    }// EOF CLASS
}