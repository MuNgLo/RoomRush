using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggler : MonoBehaviour
{
    public KeyCode _key = KeyCode.F12;
    public GameObject _toggleObject = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            _toggleObject.SetActive(!_toggleObject.activeSelf);
        }
    }
}
