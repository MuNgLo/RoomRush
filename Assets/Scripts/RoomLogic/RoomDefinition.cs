using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomLogic
{
    /// <summary>
    /// This class should only contain data. Mostly used by room manager and room driver
    /// </summary>
    [RequireComponent(typeof(RoomGroups), typeof(RoomDriver))]
    public class RoomDefinition : MonoBehaviour
    {
        public string RoomName;
        public string Author;
        public string Objective;
        public int ObjectiveCount;
        public int Tier_Lowest = -1;
        public int Tier_Highest = -1;
        public float Par_Time = 30.0f;
        public float Penatly_Fail = 0.0f;
        public float Penatly_Reset = 0.0f; // Only applied when manually reseting
        public bool ResetPlayerOnReset = false;
        public float Penalty_Hurt = 1.0f;

    }// EOF CLASS
}