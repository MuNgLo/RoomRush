using System;
using System.Collections;
using System.Collections.Generic;
using DecaMovement.Base;
using UnityEngine;

public class Core : MonoBehaviour
{
    private static Core _instance = null;
    private RoomManager _roomMan = null;
    private RunManager _runMan = null;
    private PlayerManager _playerMan = null;
    private UIStatemanager _UIStateMan = null;

    public static Core Instance { get => _instance; private set => _instance = value; }
    public Transform Avatar { get => _playerMan.PlayerAvatar; internal set { } }

    public PLAYERSTATE PlayerState { get => _playerMan.PlayerState; internal set { } }

    internal float RunRoomTime(float deltaTime)
    {
        return _roomMan.RunRoomTime(deltaTime);
    }

    public ROOMSTATE RoomState { get
        {
            if (_roomMan.CurrentRoom == null) { return ROOMSTATE.PRE; } else { return _roomMan.CurrentRoom.CurrentState; }
        }
        internal set { } }
    public float RoomTimer { get { return _roomMan.CurrentRoom._parTime; } private set { } }
    public float LifeTimer { get { return _runMan.TimerLife; } private set { } }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (!Instance) { Instance = this; }
        _roomMan = gameObject.GetComponentInChildren<RoomManager>();
        _runMan = gameObject.GetComponentInChildren<RunManager>();
        _playerMan = gameObject.GetComponentInChildren<PlayerManager>();
        _UIStateMan = GameObject.Find("UI").GetComponent<UIStatemanager>();

        // Do a check and spit error if found
        if(!_roomMan || !_runMan || !_playerMan) { Debug.LogError("Core failed setup!"); }

        _playerMan.PlayerState = PLAYERSTATE.DEAD;
    }

    /// <summary>
    /// This is the method that should do all nescassary steps to generate a proper run
    /// </summary>
    internal void StartRun()
    {
        if(_playerMan.PlayerState == PLAYERSTATE.ALIVE)
        {
            KillRun();
        }

        _UIStateMan.GotToMenu("HUD");
        RoomLogic.RoomDriver startRoom = _roomMan.GenerateStartRoom(Vector3.zero, Vector3.forward);
        if (!startRoom)
        {
            Debug.LogError("Startroom script returned as null");
        }
        _roomMan.GenerateCorridor(startRoom.Connectionpoint);
        _playerMan.SetViewRotation(startRoom.SpawnPoint.rotation);
        _playerMan.PlayerState = PLAYERSTATE.ALIVE;
        _playerMan.SpawnPlayer(startRoom.SpawnPoint);
        _roomMan.SetRoomState(startRoom.InitialState);
        _runMan.StartTimer(60.0f); // feed how much time the player starts with
        _roomMan.ActivateRoom();
    }

    internal void KillRun()
    {
        Debug.Log("KILLING RUN!");
        _playerMan.PlayerState = PLAYERSTATE.DEAD;
        _UIStateMan.GotToMenu("MainMenu");

    }



    internal InputHandling GetInputHandling()
    {
        return _playerMan.PlayerInput;
    }
}
