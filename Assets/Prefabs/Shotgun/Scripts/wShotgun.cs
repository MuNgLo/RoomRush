using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class wShotgun : MonoBehaviour
{
    private int _fireRate = 0;
    private int _magSize = 0;
    private int _magCurrent = 0;
    private float _reloadSpeed = 1.0f;
    private float _ejectionForce = 5.0f;

    public Transform _ejectionPoint = null;
    public GameObject _flash = null;
    public GameObject _shellPrefab = null;
    public GameObject _pelletPrefab = null;
    public GameObject _muzzleEffectPrefab = null;
    public AudioClip _sfxFire = null;
    public ShotgunSwarmPatterns _patterns = null; // On Muzzle transform!

    private float _tsLastFired = 0.0f;
    private int _flashCount = 0;
    private bool _isReloading = false;
    private Animator _anims = null;
    private AudioSource _audio = null;

    public object AmmoCount { get => _magCurrent; internal set { } }
    public object MagSize { get => _magSize; internal set { } }

    // Start is called before the first frame update
    void Start()
    {
        _anims = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _flash.SetActive(false);
        _fireRate = Core.Instance.Settings.Weapon.FireRate;
        _magSize = Core.Instance.Settings.Weapon.MagSize;
        _reloadSpeed = Core.Instance.Settings.Weapon.ReloadSpeed;
        _ejectionForce = Core.Instance.Settings.Weapon.EjectionForce;
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
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            if (_magCurrent < 1) { Reload(); return; }
            if (!_isReloading && Time.time > _tsLastFired + (60.0f / _fireRate))
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
       
        _magCurrent--;
        _tsLastFired = Time.time;
        Transform[] pattern = _patterns.GetPattern();
        //Spawn pellets
        for (int i = 0; i < pattern.Length; i++)
        {
            GameObject pellet = Instantiate(_pelletPrefab, _patterns.transform.position, _patterns.transform.rotation);
            pellet.transform.LookAt(pattern[i].position);
        }
        // Do flash
        _flash.SetActive(true);
        GameObject muzzleFlash = Instantiate(_muzzleEffectPrefab, _patterns.transform.position, _patterns.transform.rotation);
        muzzleFlash.transform.LookAt(pattern[0]);
        _anims.SetTrigger("Cycle");
        _audio.PlayOneShot(_sfxFire);
    }
    public void Eject()
    {
        GameObject shell = Instantiate(_shellPrefab, _ejectionPoint.position, _ejectionPoint.rotation);
        Vector3 pushDir = _ejectionPoint.up + _ejectionPoint.forward * -0.15f;
        shell.GetComponent<Rigidbody>().AddForce(pushDir * _ejectionForce);
        shell.GetComponent<Rigidbody>().AddTorque(_ejectionPoint.right * 70.0f);

    }
    internal void Reload()
    {
        if(_magCurrent == _magSize) { return; }
        _isReloading = true;
        StartCoroutine("ReloadCycle", _reloadSpeed);
    }
    private IEnumerator ReloadCycle(float speed)
    {
        _anims.SetTrigger("Reload");
        yield return new WaitForSeconds(speed);
        _magCurrent = _magSize;
        _anims.SetTrigger("ReloadPost");
    }
    /// <summary>
    /// This gets called from ReloadPost animation
    /// </summary>
    public void ReloadDone()
    {
        _isReloading = false;
    }
}
