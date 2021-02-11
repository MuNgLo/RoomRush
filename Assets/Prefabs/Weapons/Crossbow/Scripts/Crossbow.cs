using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomLogic;
using System;

public class Crossbow : RoomObjectBehaviour
{
    public GameObject _boltPrefab;
    public Transform _boltMountPoint = null;
    public float _damage = 10.0f;
    public float _speed = 30.0f;
    public float _firerate = 30.0f;
    private float _coolDown = 0.0f;
    private CrossbowBolt _currentBolt;

    public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
    {
        roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
        LoadBolt();
    }

    public override void RoomUpdate(float roomDeltaTime)
    {
        if(_coolDown > 0.0f) { _coolDown -= roomDeltaTime; } else
        {
            if(_currentBolt == null)
            {
                LoadBolt();
            }
        }
        
    }

    private void LoadBolt()
    {
        GameObject obj = Instantiate(_boltPrefab, _boltMountPoint.position, _boltMountPoint.rotation, _boltMountPoint);
        obj.transform.localScale = Vector3.one;
        _currentBolt = obj.GetComponent<CrossbowBolt>();
        if (_currentBolt == null)
        {
            Debug.LogError("Crossbow off cooldown but no bolt loaded!");
            return;
        }
    }

    internal void Fire()
    {
        if (_coolDown <= 0.0f)
        {
            if(_currentBolt == null)
            {
                LoadBolt();

                return;
            }
            _currentBolt.Fire(_speed, _damage);
            _coolDown = 60.0f / _firerate;
            _currentBolt = null;
        }
    }
}
