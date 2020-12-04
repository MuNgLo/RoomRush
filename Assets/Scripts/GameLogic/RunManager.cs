using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This handles timers, playerevents like hur, fails, roomclears and spawns.
/// </summary>
public class RunManager : MonoBehaviour
{
    [SerializeField]
    private float _timerLife = 0.0f;

    public float TimerLife { get => _timerLife; private set => _timerLife = value; }

    internal void StartTimer(float startLifeTime)
    {
        TimerLife = startLifeTime;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (Core.Instance.RoomState == ROOMSTATE.ACTIVE)
        {
            TimerLife -= Core.Instance.RunRoomTime(Time.deltaTime);
        }
    }

}
