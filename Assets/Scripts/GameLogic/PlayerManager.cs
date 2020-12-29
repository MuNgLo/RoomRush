using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class controls the the big changes for the player and player view. When spawning in, dying and such.
/// </summary>
[RequireComponent(typeof(InputHandling))]
public class PlayerManager : MonoBehaviour
{
    public GameObject _playerView = null;
    public GameObject _viewModelsCamera = null;
    public wShotgun _weapon = null;
    private InputHandling _playerInput = null;
    private GameObject _playerAvatar = null;

    [SerializeField]
    private bool _isInLava = false;
    [SerializeField]
    private float _tsLastLavaHit = 0.0f;
    public bool IsInLava { get => _isInLava; set { TsLastLavaHit = Time.time; _isInLava = value; } }
    public float TsLastLavaHit { get => _tsLastLavaHit; set => _tsLastLavaHit = value; }

    private PLAYERSTATE _playerState = PLAYERSTATE.DEAD;
    public PLAYERSTATE State { get => _playerState; set => ChangePlayerState(value); }
    public InputHandling PlayerInput { get => _playerInput; private set => _playerInput = value; }
    public Transform Avatar { get => _playerAvatar.transform; private set { } }
    public wShotgun Weapon { get => _weapon; private set { } }


    public GameObject _prefabPlayerAvatar = null;

    private void Update()
    {
        if(_isInLava && Time.time > TsLastLavaHit + Core.Instance.Settings.Room.LavaCoolDown)
        {
            _isInLava = false;
        }
    }

    private void Awake()
    {
        _playerInput = GetComponent<InputHandling>();
        // Playerview has the camera for the main rendering so we make sure it is active as we start up
        _playerView.SetActive(true);
        _viewModelsCamera.SetActive(false);
        State = PLAYERSTATE.DEAD;
    }

    public void SetViewRotation(Quaternion rotation)
    {
        _playerInput.SetViewRotations(rotation);
    }
    public void SpawnPlayer(Transform spawn)
    {
        _playerInput.SetViewRotations(spawn.rotation);
        _playerAvatar = Instantiate(_prefabPlayerAvatar, spawn.position, spawn.rotation);
    }

    private void ChangePlayerState(PLAYERSTATE newstate)
    {
        if(newstate == _playerState)
        {
            return;
        }

        switch (newstate)
        {
            case PLAYERSTATE.DEAD:
                GameObject.Destroy(Avatar.gameObject);
                _viewModelsCamera.SetActive(false);
                break;
            case PLAYERSTATE.ALIVE:
                _viewModelsCamera.SetActive(true);
                _weapon.gameObject.SetActive(true);
                break;
        }
        _playerState = newstate;
    }

    internal void ResetToPosition(Transform spawnPoint)
    {
        if (_playerAvatar)
        {
            _playerInput.SetViewRotations(spawnPoint.rotation);
            _playerAvatar.GetComponent<MotorAlt>().Teleport(spawnPoint);
        }
    }

    internal void ResetToSpawnPoint()
    {
        ResetToPosition(Core.Instance.Rooms.CurrentRoom.SpawnPoint);
    }
}
