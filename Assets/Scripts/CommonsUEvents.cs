using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

[System.Serializable]
public class RunStateChangeEvent : UnityEngine.Events.UnityEvent<RUNSTATE>
{

}
[System.Serializable]
public class RoomEvent : UnityEngine.Events.UnityEvent<ROOMEVENTS>
{

}
[System.Serializable]
public class TargetEvent : UnityEngine.Events.UnityEvent<TARGETSTATE>
{

}
[System.Serializable]
public class EnemyStateChange : UnityEngine.Events.UnityEvent<Enemies.ENEMYSTATE, Enemies.ENEMYSTATE>
{

}
