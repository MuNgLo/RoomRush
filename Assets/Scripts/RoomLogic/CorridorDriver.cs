using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RoomLogic
{
     [RequireComponent(typeof(BoxCollider))]
    public class CorridorDriver : MonoBehaviour
    {
        [SerializeField]
        private Transform _connectionPoint = null;
        [SerializeField]
        private DoorControls _entryDoor = null;
        [SerializeField]
        private DoorControls _exitDoor = null;
        [SerializeField]
        private bool _hasPlayerInside = false;
        [HideInInspector]
        public UnityEngine.Events.UnityEvent OnPlayerLockedInCorridor;
        [HideInInspector]
        public UnityEngine.Events.UnityEvent OnCorridorEntryOpening;


        private BoxCollider _sensorField = null; // This lets us know if player is in the corridor or not

        public Transform ConnectionPoint { get => _connectionPoint; private set => _connectionPoint = value; }

        private void Awake()
        {
            _sensorField = GetComponent<BoxCollider>();
            _sensorField.isTrigger = true;
            _exitDoor.OnDoorFullyClosed.AddListener(OnExitDoorClosed);
            _entryDoor.OnDoorOpening.AddListener(OnEntryDoorOpening);
        }

        private void OnEntryDoorOpening()
        {
            OnCorridorEntryOpening?.Invoke();
        }

        private void OnExitDoorClosed()
        {
            if (_hasPlayerInside)
            {
                _exitDoor.IsLocked = true; // This also triggers changes in door looks to look locked
                OnPlayerLockedInCorridor?.Invoke();
            }
        }

        /// <summary>
        /// This opens exit from room and to this corridor
        /// </summary>
        internal void OpenExit()
        {
            _exitDoor.IsLocked = false;
        }
        /// <summary>
        /// This opens entry to next room from this corridor
        /// </summary>
        internal void OpenEntry()
        {
            _entryDoor.IsLocked = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag != "PlayerAvatar")
            {
                Debug.LogError($"{other.name} was detected in corridor {name} as a player. That should not be. Only player avatar with the motor should be in player layer");
                return;
            }
            _hasPlayerInside = true;
        }
        private void OnTriggerExit(Collider other)
        {
            _hasPlayerInside = false;
        }
    }// EOF CLASS
}