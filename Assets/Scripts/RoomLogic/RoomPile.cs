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

        internal GameObject GetNextRoom()
        {
            if(_pile.Count < 1) { Debug.LogError("NO ROOMS ON PILE IN ROOMMANAGER!! Can't get next room"); }
            return _pile[0];
        }
    }// eof class
}
