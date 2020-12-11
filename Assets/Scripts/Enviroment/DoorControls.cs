using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(BoxCollider), typeof(Animator))]
public class DoorControls : MonoBehaviour
{
    [SerializeField]
    private bool _isLocked = true;
    private GameObject _leftDoor = null;
    public Material _matOpen;
    public Material _matClosed;
    [HideInInspector]
    public UnityEvent OnDoorFullyClosed;
    [HideInInspector]
    public UnityEvent OnDoorOpening;
    private BoxCollider _trigger = null;
    private Animator _anims = null;

    public bool IsLocked
    {
        get => _isLocked;
        set
        {
            if (value) { LockDoor(); } else { UnLockDoor(); }
        }
    }
    void Start()
    {
        _trigger = GetComponent<BoxCollider>();
        _trigger.isTrigger = true;
        _anims = GetComponent<Animator>();
        _leftDoor = transform.Find("Door.L").gameObject;
        LockDoor();
    }
    private void OnTriggerStay(Collider other)
    {
        if (IsLocked) { return; }
        _anims.SetBool("isOpen", true);
    }
    private void OnTriggerExit(Collider other)
    {
        _anims.SetBool("isOpen", false);
    }
    /// <summary>
    /// This is where we do stuff to looks when unlocked
    /// </summary>
    private void UnLockDoor()
    {
        //Debug.Log("Door Unlocked");
        Material[] mats = _leftDoor.GetComponent<MeshRenderer>().sharedMaterials;
        mats[2] = _matOpen;
        _leftDoor.GetComponent<MeshRenderer>().materials = mats;
        _isLocked = false;
    }
    /// <summary>
    /// This is where we do stuff to looks when locked
    /// </summary>
    private void LockDoor()
    {
        //Debug.Log("Door locked");
        Material[] mats = _leftDoor.GetComponent<MeshRenderer>().sharedMaterials;
            mats[2] = _matClosed;
        _leftDoor.GetComponent<MeshRenderer>().materials = mats;
        _isLocked = true;
    }

    #region ANimation callbacks
    /// <summary>
    /// This is called back to when the close animation is done
    /// </summary>
    public void DoorClosedFully()
    {
        GetComponent<AudioSource>().Stop();
        OnDoorFullyClosed?.Invoke();
    }
    public void DoorOpenFully()
    {
        GetComponent<AudioSource>().Stop();
    }
    public void DoorOpening()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();
        OnDoorOpening?.Invoke();
    }
    public void DoorClosing()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();
    }
    #endregion
}

