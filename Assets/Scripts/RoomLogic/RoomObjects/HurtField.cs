using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomLogic;
[RequireComponent(typeof(BoxCollider))]
public class HurtField : RoomObjectBehaviour
{
    public bool _RunOnRoomTime = true;
    private bool _hasPlayerInIt = false;

    public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
    {
        GetComponent<BoxCollider>().isTrigger = true;
        if (_RunOnRoomTime) { roomDriver.OnRoomUpdate.AddListener(RoomUpdate); }
    }
    #region Update Stuff
    public override void RoomUpdate(float roomDeltaTime)
    {
        if (_hasPlayerInIt)
        {
            Core.Instance.Player.IsInLava = true;
        }
    }
    void Update()
    {
        if (!_RunOnRoomTime) { RoomUpdate(Time.deltaTime); }
    }
    #endregion

    private void OnTriggerStay(Collider other)
    {
        _hasPlayerInIt = true;
    }
    private void OnTriggerExit(Collider other)
    {
        _hasPlayerInIt = false;
    }

}
