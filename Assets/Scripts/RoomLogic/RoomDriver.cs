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

        private RoomDefinition _roomDefinition = null;
        private ConditionBehaviour _condition = null;

        #region Event connections
        public EVENTCONNECTION WhenCleared = EVENTCONNECTION.CLEAR;
        public EVENTCONNECTION WhenParTimeOut = EVENTCONNECTION.PARTIMEOUT;
        public EVENTCONNECTION WhenFailed = EVENTCONNECTION.FAIL;
        #endregion

        #region events
        [HideInInspector]
        public RoomUpdateEvent OnRoomUpdate = new RoomUpdateEvent(); // Runs the Update() replacement for all RoomObjectBehaviours
        [HideInInspector]
        public RoomClearEvent OnRoomClear = new RoomClearEvent();
        [HideInInspector]
        public RoomFailEvent OnRoomFail = new RoomFailEvent();
        [HideInInspector]
        public RoomParTimeOutEvent OnRoomParTimeOut = new RoomParTimeOutEvent();
        #endregion

        private ConditionBehaviour _conditionScript = null;

        public RoomDefinition Definition { get => _roomDefinition; private set => _roomDefinition = value; }
        public ConditionBehaviour Condition { get => _condition; private set => _condition = value; }

        private void Start()
        {
            _roomDefinition = GetComponent<RoomDefinition>();
            Condition = GetComponent<ConditionBehaviour>();

            if (GetComponent<ConditionBehaviour>())
            {
                _conditionScript = GetComponent<ConditionBehaviour>();
            }
            else
            {
                Debug.LogError($"Roomdriver failed to find conditionscript in {name}");
            }
            _conditionScript.OnConditionClear.AddListener(OnConditionClear);
            _conditionScript.OnConditionFail.AddListener(OnConditionFail);
            // Init all room objects
            foreach (RoomObjectBehaviour roomObject in GetComponentsInChildren<RoomObjectBehaviour>())
            {
                roomObject.RoomObjectInit(this, _conditionScript);
            }
        }
        /// <summary>
        /// This fires the roomDriver OnRoomFail event. 
        /// It sends what is needed and roommanager listens.
        /// Only fires in active room.
        /// </summary>
        private void OnConditionFail()
        {
            if (Core.Instance.Rooms.CurrentRoomState == ROOMSTATE.ACTIVE)
            {
                if (_debug) { Debug.Log("RoomDriver()::OnConditionFail Fired!"); }
                OnRoomFail?.Invoke(_roomDefinition.Penatly_Time);
            }
        }
        /// <summary>
        /// This fires the roomDriver OnRoomClear event. 
        /// It sends what is needed and roommanager listens.
        /// Only fires in active room.
        /// </summary>
        private void OnConditionClear()
        {
            if (Core.Instance.Rooms.CurrentRoomState == ROOMSTATE.ACTIVE)
            {
                if (_debug) { Debug.Log("RoomDriver()::OnConditionClear Fired!"); }
                OnRoomClear?.Invoke();
            }
        }
        /// <summary>
        /// This runs same code as when a condition gets fulfullid
        /// Resulting in RoomClearEvent getting raised
        /// </summary>
        internal void ForceRoomClear()
        {
            OnConditionClear();
        }
        /// <summary>
        /// This runs same code as when a condition goes fail
        /// Resulting in RoomFailEvent getting raised
        /// </summary>
        internal void ForceRoomFail()
        {
            OnConditionFail();
        }
        /// <summary>
        /// This runs the RoomUpdate on all roomobjects
        /// through the OnRoomUpdate event
        /// </summary>
        /// <param name="deltaTime"></param>
        internal void RoomUpdate(float deltaTime)
        {
            OnRoomUpdate?.Invoke(deltaTime);
        }
    }//EOF CLASS
}