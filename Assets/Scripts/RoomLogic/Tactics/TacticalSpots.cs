using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomLogic.Tactical
{
    public class TacticalSpots : MonoBehaviour
    {
        [SerializeField]
        private List<HidingSpot> _hidingSpots = new List<HidingSpot>();
         

        // Start is called before the first frame update
        void Start()
        {
            Core.Tactical = this;
            foreach(HidingSpot spot in GetComponentsInChildren<HidingSpot>())
            {
                _hidingSpots.Add(spot);
            }
        }

        internal bool FindClosestHidingSpotWithRoom(Vector3 fromLocation, out Vector3 spotLocation)
        {
            spotLocation = Vector3.zero;
            List<HidingSpot> hasRoom = _hidingSpots.FindAll(p => p.HasRoom());
            float distance = 1000.0f;
            foreach(HidingSpot spot in hasRoom)
            {
                if(Vector3.Distance(spot.transform.position, fromLocation) < distance)
                {
                    spotLocation = spot.transform.position;
                    distance = Vector3.Distance(spot.transform.position, fromLocation);
                }
            }
            if(distance < 999.0f)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{_hidingSpots.Count} hidingspots";
        }
    }// EOF CLASS
}