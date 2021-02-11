using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomLogic;

public class CrossbowBolt : RoomObjectBehaviour
{
    public Light _light;
    public LayerMask _hittable;
    private bool _isActive = false;
    public float _speed = 0.0f;
    public float _damage = 0.0f;
    public Rigidbody _rb = null;

    public bool IsActive { get => _isActive; set { _isActive = value; _light.gameObject.SetActive(_isActive); } }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        Core.Instance.Rooms.CurrentRoom.OnRoomUpdate.AddListener(RoomUpdate);
    }


    public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
    {

    }

    public override void RoomUpdate(float roomDeltaTime)
    {
        if (IsActive)
        {
            float frameDistance = _speed * roomDeltaTime;
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.0002f, transform.forward, frameDistance, _hittable);
            if (hits.Length > 0)
            {
                frameDistance = hits[0].distance;
                IsActive = false;
            }
            transform.position += transform.forward * _speed * roomDeltaTime;        
        }
    }

    internal void Fire(float speed, float damage)
    {
        transform.SetParent(null);
        _speed = speed;
        _damage = damage;
        IsActive = true;
    }

    

    
}
