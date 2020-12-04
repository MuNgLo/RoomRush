using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomEvent events;
    public ROOMEVENTS OnTimeOut;
    public Transform SpawnPoint;
    public Transform Connectionpoint;
    public float _parTime = 5.0f;
    private readonly ROOMSTATE _initialState = ROOMSTATE.PRE;
    private ROOMSTATE _currentState = ROOMSTATE.PRE;

    public ROOMSTATE InitialState { get => _initialState; private set {} }

    public ROOMSTATE CurrentState { get => _currentState; set => _currentState = value; }

    internal float RoomUpdate(float deltaTime)
    {
        if(CurrentState != ROOMSTATE.ACTIVE)
        {
            Debug.Log("Trying to run time in a non active room");
            return 0.0f;
        }
        _parTime -= deltaTime;
        if (_parTime < 0.0f)
        {
            ParTimeOut();
            return _parTime;
        }
        return 0.0f;
    }

    private void ParTimeOut()
    {
        CurrentState = ROOMSTATE.POST;
        events?.Invoke(OnTimeOut);
    }
}
