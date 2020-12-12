using UnityEngine;
using UnityEngine.Events;

namespace RoomLogic
{
    public class ConditionBehaviour : MonoBehaviour
    {
        public bool _debug = false;

        [HideInInspector]
        public UnityEvent OnConditionClear;
        [HideInInspector]
        public ConditionFailEvent OnConditionFail;
        public int _clearsNeeded = 1;
        private int _clears = 0;
        public int _failsNeeded = 1;
        private int _fails = 0;

        public int Clears { get => _clears; private set => _clears = value; }



        /// <summary>
        /// This is the method called to raise the clear event
        /// </summary>
        public void RoomClear()
        {
            if (_debug) { Debug.Log($"ConditionBehaviour::RoomClear()"); }
            Clears++;
            if (Clears >= _clearsNeeded)
            {
                Clears = 0;
                OnConditionClear?.Invoke();
            }
        }

        /// <summary>
        /// This is the method called to raise the fail event
        /// </summary>
        public void RoomFail()
        {
            if (_debug) { Debug.Log($"ConditionBehaviour::RoomFail()"); }
            _fails++;
            if (_fails >= _failsNeeded)
            {
                _fails = 0;
                OnConditionFail?.Invoke();
            }
        }
    }

}
