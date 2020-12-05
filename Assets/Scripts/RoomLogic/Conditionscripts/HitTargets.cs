using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

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

        private void ReEvaluate()
        {
            int count = 0;
            foreach(Target target in _targets)
            {
                if(target.State == _StateToCount) { count++; }
                if(count >= _numberToHit)
                {
                    RaiseRoomClear();
                    return;
                }
            }
        }

        private void RaiseRoomClear()
        {
            OnConditionClear?.Invoke();
        }
    }// EOF CLASS
}
