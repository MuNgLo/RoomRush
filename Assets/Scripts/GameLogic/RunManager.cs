using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This handles timers, playerevents like hur, fails, roomclears and spawns.
/// </summary>
public class RunManager : MonoBehaviour
{
    public bool _debug = false;

    //private PRNGMarsenneTwister _RNG;
    private RunStats _stats = new RunStats();
    private float _timerLife = 0.0f;
    private RUNSTATE _state = RUNSTATE.PRERUN;
    [HideInInspector]
    public RunStateChangeEvent OnStateChange;

    public float TimerLife { get => _timerLife; private set => _timerLife = value; }
    public RUNSTATE State { get => _state; private set { _state = value; OnStateChange?.Invoke(_state); } }

    public RunStats Stats { get => _stats; private set => _stats = value; }

    private void Awake()
    {
        if (!this.enabled) { enabled = true; }
        //_RNG = new PRNGMarsenneTwister();
        //_RNG.init_genrand(Core.Instance.Settings.Runs.StartSeed);
    }


    private void Update()
    {
        
        // When in active run and a room is active this will run the room time Update
        if (State == RUNSTATE.INRUN)
        {
            float frameTimerLifeLoss = 0.0f;
            if (Core.Instance.Rooms.CurrentRoomState == ROOMSTATE.ACTIVE)
            {
                frameTimerLifeLoss -= Core.Instance.Rooms.RunRoomTime(Time.deltaTime);
            }
            TimerLife -= frameTimerLifeLoss;
            // Check if player has time left
            if (TimerLife <= 0.0f)
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
        //reset stats
        _stats = new RunStats();
        State = RUNSTATE.INRUN;

        RoomLogic.RoomDriver startRoom = Core.Instance.Rooms.GenerateStartRoom(Vector3.zero, Vector3.forward);
        if (!startRoom)
        {
            Debug.LogError("Startroom script returned as null");
        }
        Core.Instance.Rooms.PlaceCorridor(startRoom.Connectionpoint);
        Core.Instance.Player.SetViewRotation(startRoom.SpawnPoint.rotation);
        Core.Instance.Player.State = PLAYERSTATE.ALIVE;
        Core.Instance.Player.SpawnPlayer(startRoom.SpawnPoint);
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
    /// <summary>
    /// RoomManger runs this when it detects that current room is cleared
    /// </summary>
    /// <param name="gainedTime"></param>
    internal void RoomCleared(float gainedTime)
    {
        if (_debug) { Debug.Log($"RunManager::RoomCleared()  gainedTime = {gainedTime}"); }

        _timerLife = _timerLife + gainedTime;
        Stats.GainedClearTime += gainedTime;
        Stats.RoomCleared++;
        Core.Instance.Rooms.ChangeState(ROOMSTATE.POST);

        Core.Instance.Rooms.OpenExit();
    }
    internal void RoomFail(float penalty)
    {
        if (_debug) { Debug.Log($"RunManager::RoomFail() penalty = {penalty}"); }
        _timerLife -= penalty;
        Stats.GainedPenaltyTime += penalty;
        Stats.RoomFailed++;
    }

    internal void ForcedPenaltyTime(float penalty)
    {
        float remainingPenalty = penalty;
        if (Core.Instance.Rooms.CurrentRoomState == ROOMSTATE.ACTIVE)
        {
            remainingPenalty = Core.Instance.Rooms.ForcePenaltyTime(penalty);
        }
        TimerLife -= remainingPenalty;
    }
}
