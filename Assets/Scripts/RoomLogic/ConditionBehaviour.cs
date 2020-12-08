using UnityEngine;
using UnityEngine.Events;

namespace RoomLogic
{
    abstract public class ConditionBehaviour : MonoBehaviour
    {
        [HideInInspector]
        public UnityEvent OnConditionClear;
        [HideInInspector]
        public ConditionFailEvent OnConditionFail;
        abstract public void RoomClear();
        abstract public void RoomFail();

        
    }

}
