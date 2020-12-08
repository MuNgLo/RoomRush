﻿using RoomLogic.Conditionscripts;
using UnityEngine;

namespace RoomLogic.RoomObjects
{
    [RequireComponent(typeof(BoxCollider))]
    class ClearTriggerField : RoomObjectBehaviour
    {
        public LayerMask _LayerToLookFor;
        public int _CountNeeded = 1;

        private ConditionBehaviour _conditionScript;

        private void Awake()
        {
            GetComponent<BoxCollider>().enabled = false;
        }

        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
            _conditionScript = conditionScript;
        }

        public override void RoomUpdate(float roomDeltaTime)
        {
            BoxCollider box = GetComponent<BoxCollider>();
            RaycastHit[] hits = Physics.BoxCastAll(
                transform.position + box.center + Vector3.up * box.size.y, 
                box.size * 0.5f, 
                Vector3.down, 
                box.transform.rotation, 
                box.size.y, 
                _LayerToLookFor);
            if (hits.Length > 0)
            {
                if (hits.Length >= _CountNeeded) {
                    _conditionScript.RoomClear();
                }
            }
        }

    }// EOF CLASS
}