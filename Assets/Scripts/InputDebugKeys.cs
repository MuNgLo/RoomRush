using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDebugKeys : MonoBehaviour
{
    public Core _core = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        // Clear Room Key
        if(Input.GetKeyDown(KeyCode.F1))
        {
            // Triggers the clear code in the room driver as if the conditionscript raised clear event
            _core.Rooms.CurrentRoom.ForceRoomClear();
        }
        // Manipulate roomtime
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Core.Instance.Rooms.RoomTimeMultiplier += 0.05f;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Core.Instance.Rooms.RoomTimeMultiplier -= 0.05f;
        }
        // DEBUG pause thing Don't ship ??
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (Time.timeScale != 1.0f)
            {
                Time.timeScale = 1.0f;
            }
            else
            {
                Time.timeScale = 0.0f;
            }
        }
#endif
    }
}
