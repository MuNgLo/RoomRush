using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMouseRelease : MonoBehaviour
{
    void OnEnable()
    {

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
    }
}
