using System.Collections;
using System.Collections.Generic;
using RoomLogic;
using UnityEngine;

public class SingleAxisRotation : RoomLogic.RoomObjectBehaviour
{
    public bool _bRotatesOnRoomTime = true;
    public enum ROTATIONAXIS { X, Y, Z }
    public ROTATIONAXIS _aroundAxis = ROTATIONAXIS.Y;
    public float _RotationSpeed = 90.0f;

    // Update is called once per frame
    void Update()
    {
        if (_bRotatesOnRoomTime) { return; }
        float currentRotation = _RotationSpeed * Time.deltaTime;
        if (_aroundAxis == ROTATIONAXIS.Y)
        {
            this.transform.localEulerAngles =
                new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y + currentRotation, transform.localRotation.eulerAngles.z);
        }
        else if (_aroundAxis == ROTATIONAXIS.X)
        {
            this.transform.localEulerAngles =
                new Vector3(transform.localRotation.eulerAngles.x + currentRotation, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }
        else if (_aroundAxis == ROTATIONAXIS.Z)
        {
            this.transform.localEulerAngles =
                new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z + currentRotation);
        }

    }

    public override void RoomUpdate(float roomDeltaTime)
    {
        if (!_bRotatesOnRoomTime) { return; }
        float currentRotation = _RotationSpeed * roomDeltaTime;
        if (_aroundAxis == ROTATIONAXIS.Y)
        {
            this.transform.localEulerAngles =
                new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y + currentRotation, transform.localRotation.eulerAngles.z);
        }
        else if (_aroundAxis == ROTATIONAXIS.X)
        {
            this.transform.localEulerAngles =
                new Vector3(transform.localRotation.eulerAngles.x + currentRotation, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }
        else if (_aroundAxis == ROTATIONAXIS.Z)
        {
            this.transform.localEulerAngles =
                new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z + currentRotation);
        }
    }

    public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
    {
        roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
    }
}
