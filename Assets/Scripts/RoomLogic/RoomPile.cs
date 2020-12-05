using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This class contains all prefab rooms and will return a random room matching criteria that the room manager asks for
/// </summary>
namespace RoomLogic
{
    [System.Serializable]
    public class RoomPile
    {
        [SerializeField]
        List<GameObject> _pile = new List<GameObject>();

        private int _index = -1;

        internal GameObject GetNextRoom()
        {
            if(_pile.Count < 1) { Debug.LogError("NO ROOMS ON PILE IN ROOMMANAGER!! Can't get next room"); }
            _index++;
            if (_index >= _pile.Count)
            {
                _index = 0;
            }
            return _pile[_index];
        }
    }// eof class
}
