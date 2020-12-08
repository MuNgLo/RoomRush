using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class Wallwart : RoomLogic.RoomObjectBehaviour
{
    public Transform _target = null;
    public Transform _head = null;
    public float _attacRange = 5.0f;
    public float _extensionSpeed = 5.0f;

    private Animator _anims = null;
    public float _extension = 0.0f;
    private bool _isLooking = false;

    private void Awake()
    {
        _anims = GetComponent<Animator>();
    }

    public override void RoomObjectInit(RoomLogic.RoomDriver roomDriver, RoomLogic.ConditionBehaviour conditionScript)
    {
        //roomDriver.OnRoomActivated.AddListener(OnRoomActivated);
        Core.Instance.Rooms.OnRoomActivated.AddListener(OnRoomActivated);

        roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
    }

    private void OnRoomActivated(GameObject playerAvatar)
    {
        _target = playerAvatar.transform;
    }

    public override void RoomUpdate(float roomDeltaTime)
    {
        if(_attacRange > Vector3.Distance(transform.position, _target.position))
        {
            if (!_isLooking) { _isLooking = true; _anims.SetBool("isEyeOpen", true); }
            _anims.SetFloat("Extention", Mathf.Clamp01(_extension + _extensionSpeed * Time.deltaTime));
        }
        else
        {
            if (_isLooking) { _isLooking = false; _anims.SetBool("isEyeOpen", false); }
            _anims.SetFloat("Extention", Mathf.Clamp01(_extension - _extensionSpeed * Time.deltaTime));
        }
    }

}
