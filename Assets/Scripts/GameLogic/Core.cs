using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The goal here is to basically be a singleton style reference collection
/// </summary>
public class Core : MonoBehaviour
{
    private static Core _instance = null;
    [SerializeField]
    private GameSettings _settings = new GameSettings();
    private RoomManager _roomMan = null;
    private RunManager _runMan = null;
    private PlayerManager _playerMan = null;

    public static Core Instance { get => _instance; private set { } }

    internal string DebugText()
    {
        string NL = Environment.NewLine;
        string response = $"Current Run State : {Runs.State}{NL}" +
            $"Current Room State : {Rooms.CurrentRoomState}{NL}" +
            $"Current Room Time Multiplier : {Rooms.RoomTimeMultiplier}{NL}" +
            Runs.Stats.ToString();

        return response;
    }

    public GameSettings Settings { get => _settings; private set { } }
    public RoomManager Rooms { get => _roomMan; private set { } }
    public RunManager Runs { get => _runMan; private set { } }
    public PlayerManager Player { get => _playerMan; private set { } }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (!_instance) { _instance = this; }
        _roomMan = gameObject.GetComponentInChildren<RoomManager>();
        _runMan = gameObject.GetComponentInChildren<RunManager>();
        _playerMan = gameObject.GetComponentInChildren<PlayerManager>();

        // Do a check and spit error if found
        if(!Rooms || !Runs || !Player || Settings == null) { Debug.LogError("Core failed setup!"); }
    }
}
