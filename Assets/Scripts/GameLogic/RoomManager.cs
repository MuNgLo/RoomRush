using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RoomLogic;

public class RoomManager : MonoBehaviour
{
    public bool _debug = false;
    #region GameObjects
    public GameObject _prefabStartRoom = null;
    public GameObject _prefabCorridor = null;
    private GameObject _activeRooms = null; // The GO container for the currenty in play room objects
    private GameObject _corridorA = null;
    private GameObject _corridorB = null;

    
    #endregion

    [SerializeField]
    private RoomDriver _currentRoom = null;

    private ROOMSTATE _currentRoomState = ROOMSTATE.PRE;

    private float _currentRoomTime = 5.0f;
    private float _roomTimeMultiplier = 1.0f;

    private float _tsLastLavaHit = 0.0f;

    public float RoomTimer { get => _currentRoomTime; private set { } }


    [SerializeField]
    private RoomPile _roomPile = new RoomPile();

    private bool _useCorrA = true; // When using a corridor we use _corridorA if this is true. _corridorB if false.

    public RoomDriver CurrentRoom { get => _currentRoom; private set => _currentRoom = value; }
    public ROOMSTATE CurrentRoomState { get => _currentRoomState; private set => _currentRoomState = value; }
    public float CurrentRoomTime { get => _currentRoomTime; private set => _currentRoomTime = value; }
    public float RoomTimeMultiplier { get => _roomTimeMultiplier; set => _roomTimeMultiplier = Mathf.Clamp01(value); }

    [HideInInspector]
    public RoomActivatedEvent OnRoomActivated = new RoomActivatedEvent(); // Raised when room goes from pre to active
    [HideInInspector]
    public RoomClearedEvent OnRoomCleared = new RoomClearedEvent();
    [HideInInspector]
    public RoomFailedEvent OnRoomFailed = new RoomFailedEvent();



    /// <summary>
    /// Runs all RoomUpdate()
    /// Called and timed by runmanager
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    internal float RunRoomTime(float deltaTime)
    {
        bool hasTimedOut = _currentRoomTime <= 0.0f; // Did we timeout last update?
        float extraPenaltyTimeLoss = 0.0f;
        deltaTime = deltaTime * RoomTimeMultiplier; // Apply time dilation
        CurrentRoom.RoomUpdate(deltaTime);

        //Run lava hits
        if (Core.Instance.Player.IsInLava)
        {
            extraPenaltyTimeLoss += Core.Instance.Settings.Room.LavaDPS * deltaTime;
        }


        if (!hasTimedOut)
        {
            _currentRoomTime -= deltaTime + extraPenaltyTimeLoss;
            if (_currentRoomTime <= 0.0f)
            {
                CurrentRoomState = ParTimeOut();
                float remainder = _currentRoomTime;
                _currentRoomTime = 0.0f;
                return remainder;
            }
            return 0.0f;
        }
        return deltaTime + extraPenaltyTimeLoss;
    }

    private void Update()
    {
        if (_currentRoomState == ROOMSTATE.POST)
        {
            //Run lava hits
            if (Core.Instance.Player.IsInLava)
            {
                Debug.Log($"LAVA BURNS!! {Core.Instance.Settings.Room.LavaDPS * Time.deltaTime}");
                Core.Instance.Runs.ForcedPenaltyTime( Core.Instance.Settings.Room.LavaDPS * Time.deltaTime);
            }
        }
    }



    private ROOMSTATE ParTimeOut()
    {
        switch (CurrentRoom.WhenParTimeOut)
        {
            case EVENTCONNECTION.UNSET:
                break;
            case EVENTCONNECTION.CLEAR:
                _currentRoomTime = 0.0f;
                CurrentRoom.ForceRoomClear();
                break;
            case EVENTCONNECTION.FAIL:
                _currentRoomTime = 0.0f;
                CurrentRoom.OnRoomFail?.Invoke(CurrentRoom.Definition.Penatly_Fail);
                break;
            case EVENTCONNECTION.PARTIMEOUT:
                CurrentRoom.OnRoomParTimeOut?.Invoke(0.0f);
                _currentRoomTime = 0.0f;
                break;
            default:
                break;
        }

        return CurrentRoomState;
    }

    internal float ForcePenaltyTime(float penalty)
    {
        bool hasTimedOut = _currentRoomTime <= 0.0f;
        if (!hasTimedOut)
        {
            _currentRoomTime -= penalty;
            if (_currentRoomTime <= 0.0f)
            {
                CurrentRoomState = ParTimeOut();
                float remainder = _currentRoomTime;
                _currentRoomTime = 0.0f;
                return remainder;
            }
            return 0.0f;
        }
        return penalty;
    }

    /// <summary>
    /// This flips the room over to active from pre state.
    /// Room time will tic 
    /// Runmanger only calls this on startroom
    /// Normally it happens when corridor entry opens
    /// </summary>
    internal void ActivateRoom()
    {
        switch (_currentRoomState)
        {
            case ROOMSTATE.PRE:
                CurrentRoomState = ROOMSTATE.ACTIVE;
                OnRoomActivated?.Invoke(Core.Instance.Player.Avatar.gameObject);
                break;
            case ROOMSTATE.ACTIVE:
            case ROOMSTATE.POST:
                Debug.LogError("Trying to activate a non PRE state room.");
                break;
        }
    }

    internal void ChangeState(ROOMSTATE newState)
    {
        if (_debug) { Debug.Log($"RoomManager::ChangeState() newState = {newState}"); }
        CurrentRoomState = newState;
    }
        

    #region start of run things
    /// <summary>
    /// Runmanager calls this to setup the the startroom and corridors
    /// Destroys and refreshes the room container
    /// </summary>
    /// <param name="location"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public RoomDriver GenerateStartRoom(Vector3 location, Vector3 rotation)
    {
        //redo room container
        GameObject.Destroy(_activeRooms);
        _activeRooms = new GameObject("GENERATEDROOMS");
        _activeRooms.transform.position = Vector3.zero;
        _activeRooms.transform.rotation = Quaternion.identity;
        // Make the two corridors
        PrepCorridors();
        // make start room
        GameObject room = Instantiate(_prefabStartRoom, location, Quaternion.LookRotation(rotation, Vector3.up));
        room.transform.SetParent(_activeRooms.transform);
        if (!room.GetComponent<RoomDriver>())
        {
            Debug.LogError("Startroom script missing on startroom prefab!");
        }
        CurrentRoom = room.GetComponent<RoomDriver>();
        CurrentRoom.OnRoomClear.AddListener(OnRoomClearEvent);
        CurrentRoom.OnRoomFail.AddListener(OnRoomFailEvent);
        CurrentRoomTime = CurrentRoom.GetComponent<RoomDefinition>().Par_Time;
        _tsLastLavaHit = CurrentRoomTime + 100.0f;
        //CurrentRoomState = ROOMSTATE.PRE;
        return room.GetComponent<RoomDriver>();
    }
    /// <summary>
    /// Called when generating the startroom to make 2 corridors we can use throughout the run
    /// </summary>
    private void PrepCorridors()
    {
        _corridorA = Instantiate(_prefabCorridor, Vector3.zero, Quaternion.identity);
        _corridorA.transform.SetParent(_activeRooms.transform);
        _corridorA.GetComponent<CorridorDriver>().OnPlayerLockedInCorridor.AddListener(OnPlayerInCorridor);
        _corridorA.GetComponent<CorridorDriver>().OnCorridorEntryOpening.AddListener(OnCorridorEntryOpening);
        _corridorA.SetActive(false);
        _corridorB = Instantiate(_prefabCorridor, Vector3.zero, Quaternion.identity);
        _corridorB.transform.SetParent(_activeRooms.transform);
        _corridorB.GetComponent<CorridorDriver>().OnPlayerLockedInCorridor.AddListener(OnPlayerInCorridor);
        _corridorB.GetComponent<CorridorDriver>().OnCorridorEntryOpening.AddListener(OnCorridorEntryOpening);
        _corridorB.SetActive(false);
    }
    #endregion
    /// <summary>
    /// When corridor entry opens into next room we activate it.
    /// </summary>
    private void OnCorridorEntryOpening()
    {
        if (CurrentRoomState == ROOMSTATE.PRE)
        {
            ActivateRoom();
        }
    }
 
    /// <summary>
    /// This listen for when player enters a corridor and the door closes behind
    /// Then this fires. despawns teh previous room and Setsup the next room
    /// </summary>
    private void OnPlayerInCorridor()
    {
        if (_debug) { Debug.Log($"RoomManager::PlayerInCorridor() CurrentRoomState = {CurrentRoomState}"); }
        if(CurrentRoomState == ROOMSTATE.POST)
        {
            GameObject.Destroy(CurrentRoom.gameObject);
            CorridorDriver corrDRV = _corridorA.GetComponent<CorridorDriver>();
            if (_useCorrA) { corrDRV = _corridorB.GetComponent<CorridorDriver>(); }
            RoomDriver newRoom = SpawnNextRoom(corrDRV.ConnectionPoint.position, corrDRV.ConnectionPoint.rotation);
            CurrentRoomState = ROOMSTATE.PRE;
            
            PlaceCorridor(newRoom.Connectionpoint);
            OpenEntry();
        }
    }

    /// <summary>
    /// This gets the next room from the pile and instantiate it
    /// </summary>
    /// <param name="location"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public RoomDriver SpawnNextRoom(Vector3 location, Quaternion rotation)
    {
        GameObject room = Instantiate(_roomPile.GetNextRoom(), location, rotation);
        room.transform.SetParent(_activeRooms.transform);
        if (!room.GetComponent<RoomDriver>())
        {
            Debug.LogError($"RoomDriver script missing on room prefab({room.name})!");
        }
        CurrentRoom = room.GetComponent<RoomDriver>();
        CurrentRoom.OnRoomClear.AddListener(OnRoomClearEvent);
        CurrentRoom.OnRoomFail.AddListener(OnRoomFailEvent);
        CurrentRoomTime = CurrentRoom.GetComponent<RoomDefinition>().Par_Time;
        _tsLastLavaHit = CurrentRoomTime + 100.0f;

        return room.GetComponent<RoomDriver>();
    }

    /// <summary>
    /// This takes an unused corridor and places where requested
    /// </summary>
    /// <param name="tr"></param>
    public void PlaceCorridor(Transform tr)
    {
        GameObject corr = null;
        if (_useCorrA)
        {
            corr = _corridorA;
            _useCorrA = false;
        }
        else
        {
            corr = _corridorB;
            _useCorrA = true;
        }
        corr.transform.position = tr.position;
        corr.transform.rotation = tr.rotation;
        corr.SetActive(true);
    }
    /// <summary>
    /// Listens to the room driver clear event
    /// Opens the door to the corridor and informs
    /// the runmanager
    /// </summary>
    private void OnRoomClearEvent()
    {
        Core.Instance.Runs.RoomCleared(_currentRoomTime);
        OpenExit();
        OnRoomCleared?.Invoke();
    }
    /// <summary>
    /// Listens to the room driver fail event
    /// Opens the door to the corridor and informs
    /// the runmanager
    /// </summary>
    /// <param name="penalty"></param>
    private void OnRoomFailEvent(float penalty)
    {
        Core.Instance.Runs.RoomFail(penalty);
        OnRoomFailed?.Invoke();
    }

    /// <summary>
    /// Opens the exit from the current room
    /// </summary>
    internal void OpenExit()
    {
        if (!_useCorrA)
        {
            _corridorA.GetComponent<CorridorDriver>().OpenExit();
        }
        else
        {
            _corridorB.GetComponent<CorridorDriver>().OpenExit();
        }
    }
    private void OpenEntry()
    {
        if (_useCorrA)
        {
            _corridorA.GetComponent<CorridorDriver>().OpenEntry();
        }
        else
        {
            _corridorB.GetComponent<CorridorDriver>().OpenEntry();
        }
    }
}
