using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomLogic;
using RoomLogic.RoomObjects;

public class EffectsDriver : RoomObjectBehaviour
{
    public AudioClip _soundExplotion;
    public bool _runInRoomTime = true;
    public bool _canExplode = true;
    public float _pushForce = 0.5f;
    public Transform _pushPoint;
    private float _fuse = 0.0f;
    private enum GASBOTTLESTATE { NONE, BURNING, EXPLODING, BROKEN };
    private GASBOTTLESTATE _state = GASBOTTLESTATE.NONE;
    private Animator _anims;
    private AudioSource _audio;
    private Rigidbody _rb;

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
        if (!_runInRoomTime) { return; }
        switch (_state)
        {
            case GASBOTTLESTATE.BURNING:
                _fuse -= roomDeltaTime;
                _rb.AddForceAtPosition(-_pushPoint.forward * _pushForce, _pushPoint.position);

                if (_fuse < 0.0f)
                {
                    Explode();
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

    private void Update()
    {
        if (_runInRoomTime) { return; }
        switch (_state)
        {
            case GASBOTTLESTATE.BURNING:
                _fuse -= Time.deltaTime;
                _rb.AddForceAtPosition(-_pushPoint.forward * _pushForce, _pushPoint.position);

                if (_fuse < 0.0f)
                {
                    Explode();
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

    public void Explode()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, Core.Instance.Settings.Room.GasBottleExplosionRadius, Vector3.down, 0.1f);
        if(hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.GetComponent<Rigidbody>())
                {
                    hit.collider.GetComponent<Rigidbody>().AddExplosionForce(Core.Instance.Settings.Room.GasBottleExplosionForce, transform.position, Core.Instance.Settings.Room.GasBottleExplosionRadius);
                }
                if (hit.collider.GetComponent<WhenHit>() && hit.collider.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                {
                    hit.collider.GetComponent<WhenHit>().TakeHit();
                }
                if (hit.collider.GetComponent<CharacterController>())
                {
                    Core.Instance.Runs.ForcedPenaltyTime(Core.Instance.Settings.Room.GasBottlePenalty, true);
                }
                if (hit.collider.GetComponent<Target>())
                {
                    hit.collider.GetComponent<Target>().RegisterHit();
                }

            }

        }

        
    }
}
