using UnityEngine;
using UnityEngine.AI;
using RoomLogic;
using System;

namespace Enemies
{
    // Use physics raycast hit from mouse click to set agent destination
    public class NavMeshIntegration : RoomObjectBehaviour
    {
        private Transform _head = null;
        private EnemyAI _ai;

        void Awake()
        {
            foreach (CapsuleCollider coll in GetComponentsInChildren<CapsuleCollider>())
            {
                if (coll.name == "Head")
                {
                    _head = coll.transform;
                }
            }
            _ai = GetComponent<EnemyAI>();
        }
        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
        }
        public override void RoomUpdate(float roomDeltaTime)
        {
            if (_ai.Target)
            {
                _head.LookAt(_ai.Target.transform.position + Vector3.up * 1.1f);
            }
        }
        
    }// EOF CLASS
}