using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject _startRoom = null;
    public GameObject _prefabCorridor = null;
    private GameObject _activeRooms = null;
    [SerializeField]
    private Room _currentRoom = null;


    private GameObject _corridorA = null;
    [SerializeField]
    private GameObject _corridorB = null;
    [SerializeField]
    private bool _useCorrA = true; // When using a corridor we use _corridorA if this is true. _corridorB if false.

    public Room CurrentRoom { get => _currentRoom; private set => _currentRoom = value; }

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
        _corridorA.SetActive(false);
        _corridorB = Instantiate(_prefabCorridor, Vector3.zero, Quaternion.identity);
        _corridorB.transform.SetParent(_activeRooms.transform);
        _corridorB.SetActive(false);
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
    public Room GenerateStartRoom(Vector3 location, Vector3 rotation)
    {
        GameObject.Destroy(_activeRooms);
        _activeRooms = new GameObject("GENERATEDROOMS");
        _activeRooms.transform.position = Vector3.zero;
        _activeRooms.transform.rotation = Quaternion.identity;
        PrepCorridors();
        GameObject room = Instantiate(_startRoom, location, Quaternion.LookRotation(rotation, Vector3.up));
        room.transform.SetParent(_activeRooms.transform);
        if (!room.GetComponent<Room>())
        {
            Debug.LogError("Startroom script missing on startroom prefab!");
        }
        CurrentRoom = room.GetComponent<Room>();
        CurrentRoom.events.AddListener(CurrentRoomEvents);
        return room.GetComponent<Room>();
    }

    private void CurrentRoomEvents(ROOMEVENTS arg)
    {
        switch (arg)
        {
            case ROOMEVENTS.CLEAR:
                OpenExit();
                break;
            case ROOMEVENTS.FAIL:
                OpenExit();
                break;
            default:
                break;
        }
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
}
