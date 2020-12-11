using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioTierSettings _musicT1 = null;
    [SerializeField]
    private AudioTierSettings _musicT2 = null;
    [SerializeField]
    private AudioTierSettings _musicT3 = null;

    private AudioSource _audio = null;
    private enum FADESTATE { NONE, IN, OUT }
    private enum TIERSTATE { NONE, ONE, TWO, THREE }
    private FADESTATE _state = FADESTATE.NONE;
    private TIERSTATE _tierState = TIERSTATE.NONE;


    // Start is called before the first frame update
    void Start()
    {
        Core.Instance.Rooms.OnRoomActivated.AddListener(OnRoomActivated);
        Core.Instance.Rooms.OnRoomCleared.AddListener(OnRoomCleared);
        Core.Instance.Rooms.OnRoomFailed.AddListener(OnRoomFailed);
        _audio = GetComponent<AudioSource>();
    }

    private void OnRoomActivated(GameObject arg0)
    {
        MusicStart(TIERSTATE.ONE);
    }
    private void OnRoomCleared()
    {
        MusicStop();
    }
    private void OnRoomFailed()
    {
        MusicStop();
    }

    private void MusicStart(TIERSTATE tier)
    {
        if (_audio.isPlaying)
        {
            Debug.LogWarning("MusicManager::MusicStart() Can't start because audio already playing!");
            _audio.Stop();
        }
        _tierState = tier;
        StartCoroutine(FadeIn(CurrentTierMusic()));
    }
    private void MusicStop()
    {
        if (!_audio.isPlaying)
        {
            return;
        }
        StartCoroutine(FadeOut(CurrentTierMusic()));
    }

    private IEnumerator FadeIn(AudioTierSettings tier)
    {
        _audio.volume = 0.0f;
        _audio.clip = CurrentTierMusic().Clip;
        _audio.Play();
        _state = FADESTATE.IN;
        while(_audio.volume < tier.MaxVolume && _state == FADESTATE.IN)
        {
            _audio.volume += 1.0f / tier.FadeInTime * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator FadeOut(AudioTierSettings tier)
    {
        _state = FADESTATE.OUT;
        while (_audio.volume > 0.0f && _state == FADESTATE.OUT)
        {
            _audio.volume -= 1.0f / tier.FadeOutTime * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _audio.Stop();
    }

    private AudioTierSettings CurrentTierMusic()
    {
        switch (_tierState)
        {
            case TIERSTATE.ONE:
                return _musicT1;
            case TIERSTATE.TWO:
                return _musicT1;
            case TIERSTATE.THREE:
                return _musicT1;
            case TIERSTATE.NONE:
            default:
                Debug.LogError($"MusicManager::CurrentTierMusic() returning null.  _tierstate = {_tierState}");
                return null;
        }
    }
}


