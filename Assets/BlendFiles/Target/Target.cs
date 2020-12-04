using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TARGETSTATE { OFF, GREEN, RED }

public class Target : MonoBehaviour
{
    public Material _mOff;
    public Material _mGreen;
    public Material _mRed;
    public TARGETSTATE _startState = TARGETSTATE.OFF;

    private TARGETSTATE _state = TARGETSTATE.OFF;

    public TARGETSTATE State { get => _state; private set => _state = value; }

    private void Start()
    {
        ChangeState(_startState);
    }

    private void ChangeState(TARGETSTATE newState)
    {
        switch (newState)
        {
            case TARGETSTATE.OFF:
                GetComponent<MeshRenderer>().material = _mOff;
                break;
            case TARGETSTATE.GREEN:
                GetComponent<MeshRenderer>().material = _mGreen;
                break;
            case TARGETSTATE.RED:
                GetComponent<MeshRenderer>().material = _mRed;
                break;
            default:
                break;
        }
    }
}
