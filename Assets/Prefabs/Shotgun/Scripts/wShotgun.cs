using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class wShotgun : MonoBehaviour
{
    public GameObject _shellPrefab = null;
    public GameObject _pelletPrefab = null;
    public Transform _ejectionPoint = null;
    public ShotgunSwarmPatterns _patterns = null; // On Muzzle transform!
    public float _ejectionForce = 5.0f;
    public GameObject _flash = null;
    private int _flashCount = 0;
    private Animator _anims = null;

    // Start is called before the first frame update
    void Start()
    {
        _anims = GetComponent<Animator>();
        _flash.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_flash.activeSelf)
        {
            _flashCount++;
            if(_flashCount > 3)
            {
                _flashCount = 0;
                _flash.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        Transform[] pattern = _patterns.GetPattern();
        for (int i = 0; i < pattern.Length; i++)
        {
                GameObject pellet = Instantiate(_pelletPrefab, _patterns.transform.position, _patterns.transform.rotation);
                pellet.transform.LookAt(pattern[i].position);
        }
        _anims.SetTrigger("Cycle");
        _flash.SetActive(true);
    }
    public void Eject()
    {
        GameObject shell = Instantiate(_shellPrefab, _ejectionPoint.position, _ejectionPoint.rotation);
        Vector3 pushDir = _ejectionPoint.up + _ejectionPoint.forward * -0.15f;
        shell.GetComponent<Rigidbody>().AddForce(pushDir * _ejectionForce);
        shell.GetComponent<Rigidbody>().AddTorque(_ejectionPoint.right * 70.0f);

    }
}
