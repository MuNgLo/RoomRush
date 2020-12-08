using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomLogic.Conditionscripts
{

    public class TimePassed : ConditionBehaviour
    {
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
    }// EOF
}