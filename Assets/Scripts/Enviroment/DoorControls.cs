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
    [HideInInspector]
    public UnityEvent OnDoorFullyClosed;
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
        _isLocked = false;
    }
    /// <summary>
    /// This is where we do stuff to looks when locked
    /// </summary>
    private void LockDoor()
    {
        _isLocked = true;
    }

    /// <summary>
    /// This is called back to when the close animation is done
    /// </summary>
    public void DoorClosedFully()
    {
        OnDoorFullyClosed?.Invoke();
    }

    public void DoorOpening()
    {
        OnDoorOpening?.Invoke();
    }
}

