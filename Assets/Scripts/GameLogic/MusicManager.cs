using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private TIERSTATE _tierState = TIERSTATE.NONE;
    [SerializeField]
    private FADESTATE _fadeState = FADESTATE.NONE;

    private AudioSource _audioT1 = null;
    private AudioSource _audioT2 = null;
    private AudioSource _audioT3 = null;

    public AudioTierSettings MT1 { get => Core.Instance.Settings.Music.T1; private set { } }
    public AudioTierSettings MT2 { get => Core.Instance.Settings.Music.T2; private set { } }
    public AudioTierSettings MT3 { get => Core.Instance.Settings.Music.T3; private set { } }

    private enum FADESTATE { NONE, FADING }
    private enum TIERSTATE { NONE, ONE, TWO, THREE }


    // Start is called before the first frame update
    void Start()
    { 
        _audioT1 = transform.Find("Tier1").GetComponent<AudioSource>();
        _audioT2 = transform.Find("Tier2").GetComponent<AudioSource>();
        _audioT3 = transform.Find("Tier3").GetComponent<AudioSource>();
        SetupAudioTrack(_audioT1, MT1);
        SetupAudioTrack(_audioT2, MT2);
        SetupAudioTrack(_audioT3, MT3);
    }

    private void SetupAudioTrack(AudioSource audio, AudioTierSettings music)
    {
        audio.loop = true;
        audio.clip = music.Clip;
        audio.volume = 0.0f;
        audio.Play();
    }

    private void Update()
    {
        
        TIERSTATE frameTier = ResolveTier();
        if(frameTier != _tierState && _fadeState == FADESTATE.NONE)
        {
            _tierState = frameTier;
            StartCoroutine("CrossFade");
        }
    }

    private TIERSTATE ResolveTier()
    {
        if (Core.Instance.Runs.State != RUNSTATE.INRUN)
        {
            return TIERSTATE.NONE;
        }
        if(Core.Instance.Runs.TimerLife + Core.Instance.Rooms.CurrentRoomTime <= Core.Instance.Settings.Music.T3.ThresholdTime)
        {
            return TIERSTATE.THREE;
        }
        if (Core.Instance.Runs.TimerLife + Core.Instance.Rooms.CurrentRoomTime <= Core.Instance.Settings.Music.T2.ThresholdTime)
        {
            return TIERSTATE.TWO;
        }
        return TIERSTATE.ONE;
    }





    private IEnumerator CrossFade()
    {
        _fadeState = FADESTATE.FADING;

        while (_fadeState == FADESTATE.FADING)
        {
            switch (_tierState)
            {
                case TIERSTATE.NONE:
                        _audioT1.volume -= 1.0f / MT1.FadeOutTime * Time.deltaTime;
                        _audioT2.volume -= 1.0f / MT2.FadeOutTime * Time.deltaTime;
                        _audioT3.volume -= 1.0f / MT3.FadeOutTime * Time.deltaTime;
                    if(_audioT1.volume <= 0.0f && _audioT2.volume <= 0.0f && _audioT3.volume <= 0.0f)
                    {
                        _audioT1.volume = 0.0f;
                        _audioT2.volume = 0.0f;
                        _audioT3.volume = 0.0f;
                        _fadeState = FADESTATE.NONE;
                    }
                    break;
                case TIERSTATE.ONE:
                    _audioT1.volume += 1.0f / MT1.FadeInTime * Time.deltaTime;
                    _audioT2.volume -= 1.0f / MT2.FadeOutTime * Time.deltaTime;
                    _audioT3.volume -= 1.0f / MT3.FadeOutTime * Time.deltaTime;
                    if (_audioT1.volume >= MT1.MaxVolume){
                        _audioT2.volume = 0.0f;
                        _audioT3.volume = 0.0f;
                        _fadeState = FADESTATE.NONE;
                    }
                    break;
                case TIERSTATE.TWO:
                    _audioT1.volume -= 1.0f / MT1.FadeOutTime * Time.deltaTime;
                    _audioT2.volume += 1.0f / MT2.FadeInTime * Time.deltaTime;
                    _audioT3.volume -= 1.0f / MT3.FadeOutTime * Time.deltaTime;
                    if (_audioT2.volume >= MT2.MaxVolume)
                    {
                        _audioT1.volume = 0.0f;
                        _audioT3.volume = 0.0f;
                        _fadeState = FADESTATE.NONE;
                    }
                    break;
                case TIERSTATE.THREE:
                    _audioT1.volume -= 1.0f / MT1.FadeOutTime * Time.deltaTime;
                    _audioT2.volume -= 1.0f / MT2.FadeOutTime * Time.deltaTime;
                    _audioT3.volume += 1.0f / MT3.FadeInTime * Time.deltaTime;
                    if (_audioT3.volume >= MT3.MaxVolume)
                    {
                        _audioT1.volume = 0.0f;
                        _audioT2.volume = 0.0f;
                        _fadeState = FADESTATE.NONE;
                    }
                    break;
                default:
                    break;
            }
            yield return new WaitForEndOfFrame();
        }

    }

}


