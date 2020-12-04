using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorDriver : MonoBehaviour
{
    [SerializeField]
    private DoorControls _entryDoor = null;
    [SerializeField]
    private DoorControls _exitDoor = null;

    /// <summary>
    /// This opens exit from room and entry to this corridor
    /// </summary>
    internal void OpenExit()
    {
        _exitDoor.IsLocked = false;
    }
}
