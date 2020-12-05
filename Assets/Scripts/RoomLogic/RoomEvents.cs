using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace RoomLogic
{
    public class RoomEventClear : UnityEvent<float>
    {
    }
    public class RoomEventFail : UnityEvent<float>
    {
    }
    public class RoomEventParTimeOut : UnityEvent<float>
    {
    }
}
