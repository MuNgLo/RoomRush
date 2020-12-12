using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomLogic;
using RoomLogic.RoomObjects;

public class EffectsDriver : RoomObjectBehaviour
{
    public AudioClip _soundExplotion;
    public bool _canExplode = true;
    public float _pushForce = 0.5f;
    public Transform _pushPoint;
    private float _fuse = 0.0f;
    private enum GASBOTTLESTATE { NONE, BURNING, EXPLODING, BROKEN };
    private GASBOTTLESTATE _state = GASBOTTLESTATE.NONE;
    private Animator _anims;
    private AudioSource _audio;
    private Rigidbody _rb;
    /*public GameObject _NozzleLight;
    public GameObject _NozzleVFX;
    public GameObject _ExLight;
    public GameObject _ExVFX;*/

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<WhenHit>().OnHit.AddListener(Hit);
        _anims = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Hit()
    {
        if (_state != GASBOTTLESTATE.NONE) { return; }
        _state = GASBOTTLESTATE.BURNING;
        _anims.SetTrigger("Burn");
        _fuse = Core.Instance.Settings.Room.GasBottleFuse;
    }

    public override void RoomUpdate(float roomDeltaTime)
    {
        switch (_state)
        {
            case GASBOTTLESTATE.BURNING:
                _fuse -= roomDeltaTime;
                _rb.AddForceAtPosition(-_pushPoint.forward * _pushForce, _pushPoint.position);

                if (_fuse < 0.0f)
                {
                    _anims.SetTrigger("Explode");
                    _state = GASBOTTLESTATE.EXPLODING;
                }
                break;
            case GASBOTTLESTATE.EXPLODING:
                _audio.PlayOneShot(_soundExplotion);
                _state = GASBOTTLESTATE.BROKEN;
                break;
            case GASBOTTLESTATE.NONE:
            case GASBOTTLESTATE.BROKEN:
            default:
                break;
        }
    }

    public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
    {
        roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
    }
}
