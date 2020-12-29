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
        public bool _clearOnDead = true;
        private float _life = 100.0f;
        private float _moveSpeed = 5.0f;
        private float _currentMoveSpeed = 0.0f;
        private Vector3 _lastLocation = Vector3.zero;

        public ENEMYSTATE State { get => _state; set { ChangeState(value); } }
        public float CurrentMoveSpeed { get => _currentMoveSpeed; set => _currentMoveSpeed = value; }

        private ConditionBehaviour _conditionScrip;
        private NavMeshAgent _navAgent;
        private NavMeshIntegration _navIntegration;
        private RagDollControls _ragControls;

        [HideInInspector]
        internal EnemyStateChange OnStateChange = new EnemyStateChange();

        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _navIntegration = GetComponent<NavMeshIntegration>();
            _ragControls = GetComponent<RagDollControls>();
        }

        internal void TakeHit(Vector3 force, float damage)
        {
            if (Core.Instance.Rooms.CurrentRoomState == ROOMSTATE.ACTIVE && State != ENEMYSTATE.DEAD)
            {
                _ragControls.PushHip(force);
                _life -= damage;
                if(_life <= 0.0f)
                {
                    ChangeState(ENEMYSTATE.DEAD);
                    return;
                }

                if (State != ENEMYSTATE.STUNNED)
                {
                    ChangeState(ENEMYSTATE.STUNNED);
                    StartCoroutine("StunTimer");
                }
            }
        }
        IEnumerator StunTimer()
        {
            yield return new WaitForSeconds(Core.Instance.Settings.Enemies.StunTime);
            if (Core.Instance.Rooms.CurrentRoomState == ROOMSTATE.ACTIVE)
            {
                if(State == ENEMYSTATE.STUNNED)
                {
                    ChangeState(ENEMYSTATE.IDLE);
                }
            }
        }

        internal void ChangeState(ENEMYSTATE newState)
        {
            if(_state == newState) { return; }
            OnStateChange?.Invoke(newState, _state);
            _state = newState;
            switch (newState)
            {
                case ENEMYSTATE.INACTIVE:
                    _navAgent.isStopped = true;
                    break;
                case ENEMYSTATE.IDLE:
                    _ragControls.MakeAllKinematic();
                    _navAgent.isStopped = true;
                    break;
                case ENEMYSTATE.MOVING:
                    _ragControls.MakeAllKinematic();
                    _navAgent.isStopped = false;
                    break;
                case ENEMYSTATE.STUNNED:
                    _ragControls.MakeAllNonKinematic(true);
                    _navAgent.isStopped = true;
                    break;
                case ENEMYSTATE.DEAD:
                    _ragControls.MakeAllNonKinematic();
                    if (_clearOnDead) { _conditionScrip.RoomClear(); }
                    _navAgent.isStopped = true;
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            // calculate current movespeed over normal detal so animations can work?
            _currentMoveSpeed = Vector3.Distance(transform.position, _lastLocation) / Time.deltaTime;

            _lastLocation = transform.position;
        }

        public override void RoomUpdate(float roomDeltaTime)
        {

        }

        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            _conditionScrip = conditionScript;
            _navAgent.isStopped = true;
            _navAgent.enabled = true;
            _navIntegration.enabled = false;
            _ragControls.MakeAllKinematic();
            _state = ENEMYSTATE.INACTIVE;
            _lastLocation = transform.position;

            roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
            Core.Instance.Rooms.OnRoomActivated.AddListener(OnRoomActivated);

            _life = Core.Instance.Settings.Enemies.StartLife;
            _moveSpeed = Core.Instance.Settings.Enemies.RavMoveSpeed;
        }

        private void OnRoomActivated(GameObject playerAvatar)
        {
            ChangeState(ENEMYSTATE.IDLE);
        }
    }// EOF CLASS
}