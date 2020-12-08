using UnityEngine;

namespace RoomLogic.Conditionscripts
{
    /// <summary>
    /// This lets room objects fire RoomClear/Fail but wait until count before it raises the condition event.
    /// </summary>
    class Counts : ConditionBehaviour
    {
        public bool _debug = false;
        public int _clearsNeeded = 1;
        private int _clears = 0;
        public int _failsNeeded = 1;
        private int _fails = 0;
        override public void RoomClear()
        {
            if (_debug) { Debug.Log("Counts::RoomClear()"); }
            _clears++;
            if (_clears >= _clearsNeeded)
            {
                _clears = 0;
                OnConditionClear?.Invoke();
            }
        }
        override public void RoomFail()
        {
            if (_debug) { Debug.Log("Counts::RoomFail()"); }
            _fails++;
            if (_fails >= _failsNeeded)
            {
                _fails = 0;
                OnConditionFail?.Invoke();
            }
        }
    }// EOF CLASS
}
