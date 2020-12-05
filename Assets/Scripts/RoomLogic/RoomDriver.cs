using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RoomLogic
{
    [RequireComponent(typeof(RoomDefinition), typeof(RoomGroups))]
    public class RoomDriver : MonoBehaviour
    {



        #region events
        public RoomEventClear OnRoomClear = new RoomEventClear();
        public RoomEventClear OnRoomFail = new RoomEventClear();
        public RoomEventClear OnRoomParTimeOut = new RoomEventClear();
        #endregion

        #region Event connections
        public EVENTCONNECTION WhenCleared = EVENTCONNECTION.CLEAR;
        public EVENTCONNECTION WhenParTimeOut = EVENTCONNECTION.PARTIMEOUT;
        public EVENTCONNECTION WhenFailed = EVENTCONNECTION.FAIL;
        #endregion

        public Transform SpawnPoint;
        public Transform Connectionpoint;
        public float _parTime = 5.0f;
        private readonly ROOMSTATE _initialState = ROOMSTATE.PRE;
        private ROOMSTATE _currentState = ROOMSTATE.PRE;

        public ROOMSTATE InitialState { get => _initialState; private set { } }

        public ROOMSTATE CurrentState { get => _currentState; set => _currentState = value; }

        internal float RoomUpdate(float deltaTime)
        {
            if (CurrentState != ROOMSTATE.ACTIVE)
            {
                Debug.Log("Trying to run time in a non active room");
                return 0.0f;
            }
            _parTime -= deltaTime;
            if (_parTime < 0.0f)
            {
                ParTimeOut();
                return _parTime;
            }
            return 0.0f;
        }

        private void ParTimeOut()
        {
            switch (WhenParTimeOut)
            {
                case EVENTCONNECTION.UNSET:
                    break;
                case EVENTCONNECTION.CLEAR:
                    CurrentState = ROOMSTATE.POST;
                    OnRoomClear?.Invoke(0.0f);
                    break;
                case EVENTCONNECTION.FAIL:
                    break;
                case EVENTCONNECTION.PARTIMEOUT:
                    OnRoomParTimeOut?.Invoke(0.0f);
                    break;
                case EVENTCONNECTION.PENALTY:
                    break;
                default:
                    break;
            }

            
        }
    }//EOF CLASS
}