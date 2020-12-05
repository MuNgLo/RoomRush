using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class Target : MonoBehaviour
{
    public Material _mOff;
    public Material _mGreen;
    public Material _mRed;
    public TARGETSTATE _startState = TARGETSTATE.OFF;
    public TARGETHITREACTION _whenHit = TARGETHITREACTION.TOGGLE;
    public GameObject _redLight = null;
    public GameObject _greenLight = null;

    private TARGETSTATE _state = TARGETSTATE.OFF;
    private BoxCollider _coll = null;
    public TARGETSTATE State { get => _state; private set => _state = value; }
    [HideInInspector]
    public TargetEvent OnTargetStateChange;
    private float _lasthit = 0.0f;

    private void Awake()
    {
        _coll = GetComponent<BoxCollider>();
        _coll.isTrigger = false;
    }

    private void Start()
    {
        _state = _startState;
        switch (State)
        {
            case TARGETSTATE.OFF:
                GetComponent<MeshRenderer>().material = _mOff;
                _redLight.SetActive(false);
                _greenLight.SetActive(false);
                break;
            case TARGETSTATE.GREEN:
                GetComponent<MeshRenderer>().material = _mGreen;
                _redLight.SetActive(false);
                _greenLight.SetActive(true);
                break;
            case TARGETSTATE.RED:
                GetComponent<MeshRenderer>().material = _mRed;
                _greenLight.SetActive(false);
                _redLight.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void ChangeState(TARGETSTATE newState)
    {
        if(State == newState) { return; }
        State = newState;
        switch (State)
        {
            case TARGETSTATE.OFF:
                GetComponent<MeshRenderer>().material = _mOff;
                _redLight.SetActive(false);
                _greenLight.SetActive(false);
                break;
            case TARGETSTATE.GREEN:
                GetComponent<MeshRenderer>().material = _mGreen;
                _redLight.SetActive(false);
                _greenLight.SetActive(true);
                break;
            case TARGETSTATE.RED:
                GetComponent<MeshRenderer>().material = _mRed;
                _greenLight.SetActive(false);
                _redLight.SetActive(true);
                break;
            default:
                break;
        }
        OnTargetStateChange?.Invoke(State);
    }


    internal void RegisterHit()
    {
        if (Time.time < _lasthit + 0.1f) { return; }
        switch (_whenHit)
        {
            case TARGETHITREACTION.OFF:
                ChangeState(TARGETSTATE.OFF);
                break;
            case TARGETHITREACTION.GREEN:
                ChangeState(TARGETSTATE.GREEN);
                break;
            case TARGETHITREACTION.RED:
                ChangeState(TARGETSTATE.RED);
                break;
            case TARGETHITREACTION.TOGGLE:
                if(State == TARGETSTATE.OFF) { break; }
                if(State == TARGETSTATE.RED) { ChangeState(TARGETSTATE.GREEN); break; }
                ChangeState(TARGETSTATE.RED);
                break;
            default:
                break;
        }
        _lasthit = Time.time;
    }
}
