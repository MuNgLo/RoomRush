using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace RoomLogic
{
    /// <summary>
    /// Float value here is used as partime remainder
    /// </summary>
    [System.Serializable]
    public class RoomClearEvent : UnityEvent
    {
    }
    /// <summary>
    /// Float value here is used as the fail penalty
    /// </summary>
    [System.Serializable]
    public class RoomFailEvent : UnityEvent<float>
    {
    }
    /// <summary>
    /// This sidesteps Update() and allows to run a special delta on room objects
    /// </summary>
    [System.Serializable]
    public class RoomUpdateEvent : UnityEvent<float>
    {
    }
    [System.Serializable]
    /// <summary>
    /// Float value here is used as the fail penalty
    /// </summary>
    public class RoomParTimeOutEvent : UnityEvent<float>
    {
    }
    [System.Serializable]
    public class ConditionFailEvent : UnityEngine.Events.UnityEvent
    {

    }
    /// <summary>
    /// Send the player avatar as the gameobject
    /// </summary>
    [System.Serializable]
    public class RoomActivatedEvent : UnityEngine.Events.UnityEvent<UnityEngine.GameObject>
    {

    }
}
