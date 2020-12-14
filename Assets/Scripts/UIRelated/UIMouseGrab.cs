using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMouseGrab : MonoBehaviour
{
    void OnEnable()
    {

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        
    }
}
