using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomLogic;

public class RoomManager : MonoBehaviour
{
    public GameObject _startRoom = null;
    public GameObject _prefabCorridor = null;
    private GameObject _activeRooms = null; // The GO container for the currenty in play room objects
    [SerializeField]
    private RoomDriver _currentRoom = null;

    [SerializeField]
    private RoomPile _roomPile = new RoomPile();

    private GameObject _corridorA = null;
    private GameObject _corridorB = null;
    private bool _useCorrA = true; // When using a corridor we use _corridorA if this is true. _corridorB if false.

    public RoomDriver CurrentRoom { get => _currentRoom; private set => _currentRoom = value; }

   


    internal float RunRoomTime(float deltaTime)
    {
        return CurrentRoom.RoomUpdate(deltaTime);
    }

  

    /// <summary>
    /// Call this at start of a run to make 2 corridors we can use throughout the run
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

    /// <summary>
    /// When corridor entry opens into next room we activate it.
    /// </summary>
    private void OnCorridorEntryOpening()
    {
        Debug.Log("laskdj");
        if (CurrentRoom.CurrentState == ROOMSTATE.PRE)
        {
            ActivateRoom();
        }
    }

    private void OnPlayerInCorridor()
    {
        if(CurrentRoom.CurrentState == ROOMSTATE.POST)
        {
            GameObject.Destroy(CurrentRoom.gameObject);
            CorridorDriver corrDRV = _corridorA.GetComponent<CorridorDriver>();
            if (_useCorrA) { corrDRV = _corridorB.GetComponent<CorridorDriver>(); }
            RoomDriver newRoom = SpawnNextRoom(corrDRV.ConnectionPoint.position, corrDRV.ConnectionPoint.rotation);
            GenerateCorridor(newRoom.Connectionpoint);
            OpenEntry();
        }
    }

    public RoomDriver SpawnNextRoom(Vector3 location, Vector3 rotation)
    {
        return SpawnNextRoom(location, Quaternion.LookRotation(rotation, Vector3.up));
    }
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
        return room.GetComponent<RoomDriver>();
    }


    public void GenerateCorridor(Transform tr)
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
    public RoomDriver GenerateStartRoom(Vector3 location, Vector3 rotation)
    {
        GameObject.Destroy(_activeRooms);
        _activeRooms = new GameObject("GENERATEDROOMS");
        _activeRooms.transform.position = Vector3.zero;
        _activeRooms.transform.rotation = Quaternion.identity;
        PrepCorridors();
        GameObject room = Instantiate(_startRoom, location, Quaternion.LookRotation(rotation, Vector3.up));
        room.transform.SetParent(_activeRooms.transform);
        if (!room.GetComponent<RoomDriver>())
        {
            Debug.LogError("Startroom script missing on startroom prefab!");
        }
        CurrentRoom = room.GetComponent<RoomDriver>();
        CurrentRoom.OnRoomClear.AddListener(OnRoomClearEvent);
        CurrentRoom.OnRoomFail.AddListener(OnRoomFailEvent);
        return room.GetComponent<RoomDriver>();
    }

    private void OnRoomClearEvent(float arg)
    {
        OpenExit();
    }
    private void OnRoomFailEvent(float arg)
    {
        OpenExit();
    }
    internal void ActivateRoom()
    {
        switch (CurrentRoom.CurrentState)
        {
            case ROOMSTATE.PRE:
                SetRoomState(ROOMSTATE.ACTIVE);
                break;
            case ROOMSTATE.ACTIVE:
            case ROOMSTATE.POST:
                Debug.LogError("Trying to activate a non PRE state room.");
                break;
        }
    }

    internal void SetRoomState(ROOMSTATE initialState)
    {
        CurrentRoom.CurrentState = initialState;
    }
    private void OpenExit()
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
