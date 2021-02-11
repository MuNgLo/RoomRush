using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using RoomLogic;
using System;

namespace Enemies
{
    public class EnemyAIRanged : RoomObjectBehaviour
    {
        [SerializeField]
        public EnemyEyes Eyes;// = new EnemyEyes();

        private int _nextUpdateIn = 0;
        private GameObject _targetPlayer = null;
        private Vector3 _targetLocation = Vector3.zero;
        private ENEMYSTATE State { get => _eState.State; set { } }

        public GameObject TargetPlayer { get => _targetPlayer; private set => _targetPlayer = value; }
        public Vector3 TargetLocation { get => _targetLocation; private set => _targetLocation = value; }

        public Crossbow _crossBow;

        private EnemyState _eState = null;
        private NavMeshAgent _navAgent = null;
        //private EnemyAction _actions = null;
        private ConditionBehaviour _condition = null;
        private RoomDriver _room = null;

        private void Awake()
        {
            _eState = GetComponent<EnemyState>();
            _navAgent = GetComponent<NavMeshAgent>();
            //_actions = GetComponent<EnemyAction>();

            //Eyes = new EnemyEyes(GetComponent<RagDollControls>().GetHeadTransform().Find("Eyes"));
        }

        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            _room = roomDriver;
            _condition = conditionScript;
            _room.OnRoomUpdate.AddListener(RoomUpdate);
            Core.Instance.Rooms.OnRoomActivated.AddListener(OnRooMActivated);
        }
        private void OnRooMActivated(GameObject playerAvatar)
        {
            _targetPlayer = playerAvatar;
            _targetLocation = RandomNavmeshLocation();
            _eState.ChangeState(ENEMYSTATE.MOVING);
        }

        public override void RoomUpdate(float roomDeltaTime)
        {
            if (State == ENEMYSTATE.DEAD || State == ENEMYSTATE.INACTIVE || State == ENEMYSTATE.STUNNED)
            {
                // We ded Don't do shit
                return;
            }

            // Check if AI needs update
            _nextUpdateIn--;
            // Make sure we update when we arive at a location
            if(Vector3.Distance(_targetLocation, transform.position) < 0.2f)
            {
                _eState.ChangeState(ENEMYSTATE.IDLE);
                _nextUpdateIn -= Core.Instance.Settings.Enemies.AIUpdateRate;
            }
            // Run AI Update when needed
            if (_nextUpdateIn < 1)
            {
                _nextUpdateIn = Core.Instance.Settings.Enemies.AIUpdateRate;
                //Can we see player?
            
                if (Eyes.CanWeSee(_targetPlayer.GetComponent<CharacterController>().bounds)) {
                    _crossBow.Fire();
                    //Debug.Log("I see you!");
                }
                // Run AI Update
                float distanceToPlayer = Vector3.Distance(TargetPlayer.transform.position, transform.position); 
                // If we are close enough we should just melee 
                if (Core.Instance.Settings.Enemies.MeleeReach > distanceToPlayer)
                {
                    _eState.ChangeState(ENEMYSTATE.MELEE);
                    return;
                }
                // Check if player is looking
                bool isPlayerLooking = Vector3.Dot(TargetPlayer.transform.forward, transform.position - TargetPlayer.transform.position) > 0.1f;

                if (distanceToPlayer < Core.Instance.Settings.Enemies.ReactDistance)
                {
                    if (isPlayerLooking)
                    {
                        // Player is looking so we stalk
                        _eState.ChangeState(ENEMYSTATE.STALKING);
                        if (Core.Tactical.FindClosestHidingSpotWithRoom(transform.position, out Vector3 spot))
                        {
                            // Go hide
                            _targetLocation = spot;
                            return;
                        }
                        _targetLocation = RandomNavmeshLocation();
                    }
                    else
                    {
                        // Player is not looking so we rush them
                        _eState.ChangeState(ENEMYSTATE.RUSHING);
                    }
                    return;
                }
                _targetLocation = RandomNavmeshLocation();
                _eState.ChangeState(ENEMYSTATE.MOVING);
            }
        }

        public Vector3 RandomNavmeshLocation()
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * Core.Instance.Settings.Enemies.RoamStep;
            randomDirection += transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, Core.Instance.Settings.Enemies.RoamStep, 1))
            {
                finalPosition = hit.position;
            }
            return finalPosition;
        }

        private void OnDrawGizmos()
        {
            Eyes.OnDrawGizmos();
        }

    }// EOF CLASS
}