using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPellet : MonoBehaviour
{
    public LayerMask _hittable;
    public float _TTL = 1.0f;
    public float _speed = 10.0f;
    public int _maxBounce = 5;
    private int bounced = 0;

    private float _castRadius = 1.0f;
    private float _knockBackForce = 50.0f; // DO NOT CHANGE THIS. Instead tweak mass of other objects
    public GameObject _prefabVFXBounce = null;

    private void Start()
    {
        _castRadius = GetComponent<MeshRenderer>().bounds.extents.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        _TTL -= Time.deltaTime;
        if (_TTL <= 0.0f)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        float frameStep = _speed * Time.deltaTime;
        MoveStep(frameStep, transform.position, transform.forward);
    }

    private void MoveStep(float frameStep, Vector3 position, Vector3 direction)
    {
        transform.LookAt(transform.position + direction);
        if (Physics.SphereCast(position, _castRadius, direction, out RaycastHit hitInfo, frameStep, _hittable))
        {
            MakeHitEffect(hitInfo);
            if (hitInfo.collider.GetComponent<RoomLogic.RoomObjects.WhenHit>())
            {
                hitInfo.collider.GetComponent<RoomLogic.RoomObjects.WhenHit>().TakeHit();
            }

            if (hitInfo.collider.GetComponent<Enemies.Hitbox>())
            {
                hitInfo.collider.GetComponent<Enemies.Hitbox>().RecieveHit(transform.forward * _knockBackForce);
                DeSpawn();
                return;
            }

            // Resolve hit
            if (hitInfo.collider.GetComponent<Rigidbody>())
            {
                // Bounce of kinematic
                if (hitInfo.collider.GetComponent<Rigidbody>().isKinematic)
                {
                    Bounce(hitInfo, frameStep);
                    return;
                }
                KnockOtherRB(hitInfo);
                DeSpawn();
                return;
            }
            if (hitInfo.collider.GetComponent<RoomLogic.RoomObjects.Target>())
            {
                hitInfo.collider.GetComponent<RoomLogic.RoomObjects.Target>().RegisterHit();
            }
            Bounce(hitInfo, frameStep);
        }
        else
        {
            // No hit so move
            transform.position = position + direction * frameStep;
        }
    }

    private void Bounce(RaycastHit hit, float frameStep)
    {
        if (bounced >= _maxBounce || hit.collider.tag == "NoBounce")
        {
            DeSpawn();
            return;
        }
        float distanceAfterBounce = frameStep - hit.distance;
        Vector3 deflectVector = Vector3.Reflect(transform.forward, hit.normal);
        transform.position = hit.point + deflectVector * distanceAfterBounce;
        bounced++;
        MoveStep(distanceAfterBounce, hit.point, deflectVector);
    }

    private void DeSpawn()
    {
        GameObject.Destroy(this.gameObject);
    }

    private void KnockOtherRB(RaycastHit hit)
    {
        hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * _knockBackForce, hit.point, ForceMode.Impulse);
    }

    private void MakeHitEffect(RaycastHit hitInfo)
    {
        GameObject effects = Instantiate(_prefabVFXBounce, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
    }

}
