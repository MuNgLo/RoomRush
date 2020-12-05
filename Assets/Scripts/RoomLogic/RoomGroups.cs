using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomLogic
{
    /// <summary>
    /// This class is to control groups of objects inside the room.
    /// Basically anything non static you place in the room can be valid.
    /// </summary>
    [RequireComponent(typeof(RoomDefinition), typeof(RoomDriver))]
    public class RoomGroups : MonoBehaviour
    {
        public MonoBehaviour[] _effected;
        public GameObject[] _controlled;
        #region Event response
        public GROUPRESPONSE ConWhenOnClear = GROUPRESPONSE.NONE;
        public GROUPRESPONSE ConWhenOnFail = GROUPRESPONSE.NONE;
        public GROUPRESPONSE ConWhenOnParTimeOut = GROUPRESPONSE.NONE;
        public GROUPRESPONSE EffWhenOnClear = GROUPRESPONSE.NONE;
        public GROUPRESPONSE EffWhenOnFail = GROUPRESPONSE.NONE;
        public GROUPRESPONSE EffWhenOnParTimeOut = GROUPRESPONSE.NONE;
        #endregion
        private void Start()
        {
            RoomDriver drv = GetComponent<RoomDriver>();
            if (!drv)
            {
                Debug.LogError($"RoomGroups in {name} couldn't find the RoomDriver");

            }
            drv.OnRoomClear.AddListener(OnRoomClear);
            drv.OnRoomFail.AddListener(OnRoomFail);
            drv.OnRoomParTimeOut.AddListener(OnRoomParTimeOut);
        }

        #region Eventlisteners
        private void OnRoomParTimeOut(float arg0)
        {
            switch (ConWhenOnClear)
            {
                case GROUPRESPONSE.NONE:
                    break;
                case GROUPRESPONSE.DISABLE:
                    break;
                case GROUPRESPONSE.ENABLE:
                    break;
                case GROUPRESPONSE.TOGGLE:
                    break;
                default:
                    break;
            }
            switch (EffWhenOnClear)
            {
                case GROUPRESPONSE.NONE:
                    break;
                case GROUPRESPONSE.DISABLE:
                    break;
                case GROUPRESPONSE.ENABLE:
                    break;
                case GROUPRESPONSE.TOGGLE:
                    break;
                default:
                    break;
            }
        }
        private void OnRoomFail(float arg0)
        {
            switch (ConWhenOnClear)
            {
                case GROUPRESPONSE.NONE:
                    break;
                case GROUPRESPONSE.DISABLE:
                    break;
                case GROUPRESPONSE.ENABLE:
                    break;
                case GROUPRESPONSE.TOGGLE:
                    break;
                default:
                    break;
            }
            switch (EffWhenOnClear)
            {
                case GROUPRESPONSE.NONE:
                    break;
                case GROUPRESPONSE.DISABLE:
                    break;
                case GROUPRESPONSE.ENABLE:
                    break;
                case GROUPRESPONSE.TOGGLE:
                    break;
                default:
                    break;
            }
        }
        private void OnRoomClear(float arg0)
        {
            switch (ConWhenOnClear)
            {
                case GROUPRESPONSE.NONE:
                    break;
                case GROUPRESPONSE.DISABLE:
                    break;
                case GROUPRESPONSE.ENABLE:
                    ConEnable();
                    break;
                case GROUPRESPONSE.TOGGLE:
                    break;
                default:
                    break;
            }
            switch (EffWhenOnClear)
            {
                case GROUPRESPONSE.NONE:
                    break;
                case GROUPRESPONSE.DISABLE:
                    break;
                case GROUPRESPONSE.ENABLE:
                    break;
                case GROUPRESPONSE.TOGGLE:
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region iteratiors
        /// <summary>
        /// Iterate over array of monobehaviours on same GameObject and enable them
        /// </summary>
        internal void EffEnable()
        {
            foreach (MonoBehaviour mb in _effected)
            {
                mb.enabled = true;
            }
        }
        /// <summary>
        /// Iterate over array of monobehaviours on same GameObject and disable them
        /// </summary>
        internal void EffDisable()
        {
            foreach (MonoBehaviour mb in _effected)
            {
                mb.enabled = false;
            }
        }
        /// <summary>
         /// Iterate over array of monobehaviours on same GameObject and enable them
         /// </summary>
        internal void ConEnable()
        {
            foreach (GameObject mb in _controlled)
            {
                mb.SetActive(true);
            }
        }
        /// <summary>
        /// Iterate over array of monobehaviours on same GameObject and disable them
        /// </summary>
        internal void ConDisable()
        {
            foreach (GameObject mb in _controlled)
            {
                mb.SetActive(false);
            }
        }
        #endregion

    }// EOF CLASS
}