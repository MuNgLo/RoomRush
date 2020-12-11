using UnityEngine;
using UnityEngine.AI;
using RoomLogic;
using System;

namespace Enemies
{
    // Use physics raycast hit from mouse click to set agent destination
    public class NavMeshIntegration : RoomObjectBehaviour
    {
        public GameObject _target = null;
        public Transform _head = null;

        public float _moveSpeed = 3.0f;
        public NavMeshAgent _navAgent;
        void Awake()
        {
            foreach (CapsuleCollider coll in GetComponentsInChildren<CapsuleCollider>())
            {
                if (coll.name == "Head")
                {
                    _head = coll.transform;
                }
            }
            _navAgent = GetComponent<NavMeshAgent>();
        }
        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
            Core.Instance.Rooms.OnRoomActivated.AddListener(OnRooMActivated);
        }
        public override void RoomUpdate(float roomDeltaTime)
        {
            if (_target && _navAgent.enabled)
            {
                _head.LookAt(_target.transform.position + Vector3.up * 1.1f);
                _navAgent.destination = _target.transform.position;
            }
        }
        private void OnRooMActivated(GameObject playerAvatar)
        {
            _target = playerAvatar;
        }
    }// EOF CLASS
}