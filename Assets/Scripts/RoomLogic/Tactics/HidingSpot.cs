using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoomLogic.Tactical
{
    public class HidingSpot : MonoBehaviour
    {
        [SerializeField]
        private int _maxHiders = 2;
        [SerializeField]
        private int _currentHiders = 0;

        internal bool HasRoom()
        {
            return _currentHiders < _maxHiders;
        }

        private void LateUpdate()
        {
            _currentHiders = 0;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<Enemies.EnemyAI>())
            {
                _currentHiders++;
            }
        }
    }// EOF CLASS
}