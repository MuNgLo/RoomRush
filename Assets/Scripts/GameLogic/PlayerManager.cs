using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class controls the the big changes for the player and player view. When spawning in, dying and such.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public GameObject _playerView = null;
    public GameObject _viewModelsCamera = null;
    public GameObject _weaponmodel = null;
    [SerializeField]
    private InputHandling _playerInput = null;
    private GameObject _playerAvatar = null;

    [SerializeField]
    private PLAYERSTATE _playerState = PLAYERSTATE.DEAD;

    public PLAYERSTATE PlayerState { get => _playerState; set => ChangePlayerState(value); }
    public InputHandling PlayerInput { get => _playerInput; private set => _playerInput = value; }
    public Transform PlayerAvatar { get => _playerAvatar.transform; private set { } }

    public GameObject _prefabPlayerAvatar = null;

    private void Awake()
    {
        // Playerview has the camera for the main rendering so we make sure it is active as we start up
        _playerView.SetActive(true);
        _viewModelsCamera.SetActive(false);
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
                GameObject.Destroy(PlayerAvatar);
                _viewModelsCamera.SetActive(false);
                break;
            case PLAYERSTATE.ALIVE:
                _viewModelsCamera.SetActive(true);
                _weaponmodel.SetActive(true);
                break;
        }
        _playerState = newstate;
    }
}
