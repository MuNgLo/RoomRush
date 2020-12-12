using RoomLogic.Conditionscripts;
using UnityEngine;
using UnityEngine.Events;

namespace RoomLogic.RoomObjects
{
    class WhenHit : RoomObjectBehaviour
    {
        public bool _FireClearWhenHit = false;
        public bool _FireFailWhenHit = false;
        private ConditionBehaviour _conditionScript;
        private bool _hasBeenHit = false;
        public UnityEvent OnHit;


        public void TakeHit()
        {
            if (_hasBeenHit) { return; }
            _hasBeenHit = true;
            OnHit?.Invoke();
            if (_FireClearWhenHit) { _conditionScript.RoomClear(); }
            if (_FireFailWhenHit) { _conditionScript.RoomFail(); }
        }

        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            _conditionScript = conditionScript;
        }

        public override void RoomUpdate(float roomDeltaTime)
        {
        }
    }// EOF CLASS
}
