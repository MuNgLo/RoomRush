using System.Collections.Generic;
using UnityEngine;
using RoomLogic.RoomObjects;

namespace RoomLogic.Conditionscripts
{
    class HitTargets : ConditionBehaviour
    {
        [SerializeField]
        private int _numberToHit = 1;
        public TARGETSTATE _StateToCount = TARGETSTATE.GREEN;

        [SerializeField]
        private List<Target> _targets = new List<Target>();
        
        private void Start()
        {
            foreach(Target target in GetComponentsInChildren<Target>())
            {
                _targets.Add(target);
                target.OnTargetStateChange.AddListener(OnTargetChange);
            }
        }

        private void OnTargetChange(TARGETSTATE state)
        {
            if(state == _StateToCount)
            {
                ReEvaluate();
            }
        }

        public void ReEvaluate()
        {
            int count = 0;
            foreach(Target target in _targets)
            {
                if(target.State == _StateToCount) { count++; }
                if(count >= _numberToHit)
                {
                    RoomClear();
                    return;
                }
            }
        }
        /// <summary>
        /// This is the method called to raise the clear event
        /// </summary>
        override public void RoomClear()
        {
            OnConditionClear?.Invoke();
        }
        /// <summary>
        /// This is the method called to raise the fail event
        /// </summary>
        override public void RoomFail()
        {
            OnConditionFail?.Invoke();
        }

    }// EOF CLASS
}
