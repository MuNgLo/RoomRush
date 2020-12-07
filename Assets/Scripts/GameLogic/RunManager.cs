using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This handles timers, playerevents like hur, fails, roomclears and spawns.
/// </summary>
public class RunManager : MonoBehaviour
{
    private PRNGMarsenneTwister _RNG;
    private RunStats _stats = new RunStats();
    private float _timerLife = 0.0f;
    private RUNSTATE _state = RUNSTATE.PRERUN;

    public RunStateChangeEvent OnStateChange;

    public float TimerLife { get => _timerLife; private set => _timerLife = value; }
    public RUNSTATE State { get => _state; private set { _state = value; OnStateChange?.Invoke(_state); } }

    public RunStats Stats { get => _stats; private set => _stats = value; }

    private void Awake()
    {
        _RNG = new PRNGMarsenneTwister();
        _RNG.init_genrand(Core.Instance.Settings.Runs.StartSeed);
    }


    private void Update()
    {
        // When in active run and a room is active this will run the room time Update
        if (State == RUNSTATE.INRUN)
        {
            if (Core.Instance.Rooms.CurrentRoom.CurrentRoomState == ROOMSTATE.ACTIVE)
            {
                TimerLife -= Core.Instance.Rooms.RunRoomTime(Time.deltaTime);
            }
            // Chekc if player has time left
            if(TimerLife <= 0.0f)
            {
                EndRun();
            }
        }
    }

    /// <summary>
    /// This is the method that should do all nescassary steps to generate a proper run
    /// </summary>
    internal void StartRun()
    {
        if (Core.Instance.Player.State == PLAYERSTATE.ALIVE)
        {
            Debug.LogWarning("RunManager() Trying to start a run while player is alive");
        }
        State = RUNSTATE.INRUN;

        RoomLogic.RoomDriver startRoom = Core.Instance.Rooms.GenerateStartRoom(Vector3.zero, Vector3.forward);
        if (!startRoom)
        {
            Debug.LogError("Startroom script returned as null");
        }
        Core.Instance.Rooms.GenerateCorridor(startRoom.Connectionpoint);
        Core.Instance.Player.SetViewRotation(startRoom.SpawnPoint.rotation);
        Core.Instance.Player.State = PLAYERSTATE.ALIVE;
        Core.Instance.Player.SpawnPlayer(startRoom.SpawnPoint);
        Core.Instance.Rooms.SetRoomState(startRoom.InitialState);
        TimerLife = Core.Instance.Settings.Runs.StartTime; // how much time the player starts with
        Core.Instance.Rooms.ActivateRoom();
    }

    internal void ResetRun()
    {
        if(State == RUNSTATE.INRUN) { EndRun(); }
        StartRun();
    }

    internal void EndRun()
    {
        Debug.Log("KILLING RUN!");
        State = RUNSTATE.POSTRUN;
        Core.Instance.Player.State = PLAYERSTATE.DEAD;
    }

    internal void RoomClear(float arg)
    {
        _timerLife += arg;
        Stats.GainedClearTime += arg;
        Stats.RoomCleared++;
    }
}
