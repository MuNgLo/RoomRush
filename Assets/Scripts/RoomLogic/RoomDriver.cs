using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RoomLogic
{
    [RequireComponent(typeof(RoomGroups), typeof(RoomDefinition))]
    public class RoomDriver : MonoBehaviour
    {
        public bool _debug = false;
        // rework these to auto and verbose
        public Transform SpawnPoint;
        public Transform Connectionpoint;

        #region Event connections
        public EVENTCONNECTION WhenCleared = EVENTCONNECTION.CLEAR;
        public EVENTCONNECTION WhenParTimeOut = EVENTCONNECTION.PARTIMEOUT;
        public EVENTCONNECTION WhenFailed = EVENTCONNECTION.FAIL;
        #endregion

        #region events
        [HideInInspector]
        public RoomUpdateEvent OnRoomUpdate = new RoomUpdateEvent(); // Runs the Update() replacement for all RoomObjectBehaviours
        [HideInInspector]
        public RoomActivatedEvent OnRoomActivated = new RoomActivatedEvent();
        [HideInInspector]
        public RoomClearEvent OnRoomClear = new RoomClearEvent();
        [HideInInspector]
        public RoomFailEvent OnRoomFail = new RoomFailEvent();
        [HideInInspector]
        public RoomParTimeOutEvent OnRoomParTimeOut = new RoomParTimeOutEvent();
        #endregion

        public ROOMSTATE InitialState { get => _initialState; private set { } }
        public ROOMSTATE CurrentRoomState { get => _currentState; set => _currentState = value; }
        public float RoomTimer { get => _currentRoomTime; private set { } }

        private float _currentRoomTime = 5.0f;
        private readonly ROOMSTATE _initialState = ROOMSTATE.PRE;
        private ROOMSTATE _currentState = ROOMSTATE.PRE;
        private ConditionBehaviour _conditionScript = null;

        private void Start()
        {
            _currentRoomTime = GetComponent<RoomDefinition>().Par_Time;
            if (GetComponent<ConditionBehaviour>())
            {
                _conditionScript = GetComponent<ConditionBehaviour>();
            }
            else
            {
                Debug.LogError($"Roomdriver failed to find conditionscript in {name}");
            }
            _conditionScript.OnConditionClear.AddListener(OnConditionClear);
            _conditionScript.OnConditionFail.AddListener(OnConditionfail);
            // Init all room objects
            foreach (RoomObjectBehaviour roomObject in GetComponentsInChildren<RoomObjectBehaviour>())
            {
                roomObject.RoomObjectInit(this, _conditionScript);
            }
        }
        /// <summary>
        /// This fires the roomDriver roomfail event. It sends what is needed and roommanager listens, do changes to room and informs runmanager about the fail and values.
        /// </summary>
        private void OnConditionfail()
        {
            if (_debug) { Debug.Log("RoomDriver()::OnConditionFail Fired!"); }
            CurrentRoomState = ROOMSTATE.POST;
            OnRoomFail?.Invoke(GetComponent<RoomDefinition>().Penatly_Time);
        }
        private void OnConditionClear()
        {
            if (_debug) { Debug.Log("RoomDriver()::OnConditionClear Fired!"); }
            CurrentRoomState = ROOMSTATE.POST;
            OnRoomClear?.Invoke(_currentRoomTime);
        }

        internal float RoomUpdate(float deltaTime)
        {
            if (CurrentRoomState != ROOMSTATE.ACTIVE)
            {
                Debug.Log("Trying to run time in a non active room");
                return 0.0f;
            }
            OnRoomUpdate?.Invoke(deltaTime);

            bool hasTimedOut = _currentRoomTime <= 0.0f; // Did we timeout last update?
            if (hasTimedOut)
            {
                return deltaTime;
            }
            else
            {
                _currentRoomTime -= deltaTime; 
                if (_currentRoomTime <= 0.0f)
                {
                    ParTimeOut();
                    float remainder = _currentRoomTime;
                    _currentRoomTime = 0.0f;
                    return remainder;
                }
                return 0.0f;
            }
        }
        internal void ActivateRoom()
        {
            switch (_currentState)
            {
                case ROOMSTATE.PRE:
                    SetRoomState(ROOMSTATE.ACTIVE);
                    OnRoomActivated?.Invoke(Core.Instance.Player.Avatar.gameObject);
                    break;
                case ROOMSTATE.ACTIVE:
                case ROOMSTATE.POST:
                    Debug.LogError("Trying to activate a non PRE state room.");
                    break;
            }
        }
        internal void SetRoomState(ROOMSTATE initialState)
        {
            _currentState = initialState;
        }
        private void ParTimeOut()
        {
            switch (WhenParTimeOut)
            {
                case EVENTCONNECTION.UNSET:
                    break;
                case EVENTCONNECTION.CLEAR:
                    _currentRoomTime = 0.0f;
                    CurrentRoomState = ROOMSTATE.POST;
                    OnRoomClear?.Invoke(0.0f);
                    break;
                case EVENTCONNECTION.FAIL:
                    _currentRoomTime = 0.0f;
                    CurrentRoomState = ROOMSTATE.POST;
                    OnRoomFail?.Invoke(GetComponent<RoomDefinition>().Penatly_Time);
                    break;
                case EVENTCONNECTION.PARTIMEOUT:
                    _currentRoomTime = 0.0f;
                    OnRoomParTimeOut?.Invoke(0.0f);
                    break;
                default:
                    break;
            }

            
        }
    }//EOF CLASS
}