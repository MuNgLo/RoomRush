using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the only one the UI should make calls into. 
/// So everything in here needs to be single public method for a single functionality. 
/// Anything more complex we do elsewhere.
/// </summary>
public class UIAPI : MonoBehaviour
{
    private Core _core = null;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _core = Core.Instance;
    }


    public void StartRun()
    {
        _core.Runs.StartRun();
    }

    public float RoomTime()
    {
        return _core.Rooms.CurrentRoom.RoomTimer;
    }
}
