using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider),typeof(Animator))]
public class DoorControls : MonoBehaviour
{


    private BoxCollider _trigger = null;
    private Animator _anims = null;
    // Start is called before the first frame update
    void Start()
    {
        _trigger = GetComponent<BoxCollider>();
        _trigger.isTrigger = true;
        _anims = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        _anims.SetBool("isOpen", true);
    }
    private void OnTriggerExit(Collider other)
    {
        _anims.SetBool("isOpen", false);
    }
}
