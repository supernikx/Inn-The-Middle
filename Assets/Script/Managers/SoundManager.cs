using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public bool _SoundActive;
    public bool SoundActive
    {
        get
        {
            return _SoundActive;
        }
        set
        {
            _SoundActive = value;
            if (!BoardManager.Instance.pause)
            {
                if (SoundActive && !MusicAudioSource.isPlaying)
                {
                    MusicAudioSource.Play();
                    PlayerPrefs.SetInt("Sound", 1);
                }
                else if (!SoundActive && MusicAudioSource.isPlaying)
                {
                    MusicAudioSource.Pause();
                    PlayerPrefs.SetInt("Sound", 0);
                }
            }
            else
            {
                if (SoundActive)
                {
                    PlayerPrefs.SetInt("Sound", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("Sound", 0);
                }
            }            
        }
    }
    public AudioSource MusicAudioSource;
    public AudioSource GeneralAudioSource;

    [Header("Clips Menu")]
    public AudioClip buttonselection;
    public AudioClip selectnextbutton;
    public AudioClip togglebutton;
    public AudioClip factionslected;

    private void OnEnable()
    {
        EventManager.OnPause += OnGamePause;
        EventManager.OnUnPause += OnGameUnPause;
    }

    private void OnDisable()
    {
        EventManager.OnPause -= OnGamePause;
        EventManager.OnUnPause -= OnGameUnPause;
    }

    private void OnGamePause()
    {
        if (SoundActive)
            MusicAudioSource.Pause();
    }

    private void OnGameUnPause()
    {
        if (SoundActive)
            MusicAudioSource.Play();
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        int soundindex = PlayerPrefs.GetInt("Sound", -1);
        if (soundindex == -1)
        {
            PlayerPrefs.SetInt("Sound", 1);
            SoundActive = true;
        }
        else
        {
            switch (soundindex)
            {
                case 0:
                    SoundActive = false;
                    break;
                case 1:
                    SoundActive = true;
                    break;
                default:
                    Debug.Log("Dato salvato non valido");
                    break;
            }
        }
        BoardManager.Instance.uiManager.UpdateSoundUI();
    }

    public void ActiveDeactiveSound()
    {
        SoundActive = !SoundActive;
    }

    public void ButtonSelection()
    {
        if (SoundActive)
        {
            if (buttonselection != null)
            {
                GeneralAudioSource.clip = buttonselection;
                GeneralAudioSource.Play();
            }
        }
    }

    public void SelectNextButton()
    {
        if (GeneralAudioSource != null)
        {
            if (SoundActive)
            {
                if (selectnextbutton != null)
                {
                    GeneralAudioSource.clip = selectnextbutton;
                    GeneralAudioSource.Play();
                }
            }
        }
    }

    public void ToggleButton()
    {
        if (SoundActive)
        {
            if (togglebutton != null)
            {
                GeneralAudioSource.clip = togglebutton;
                GeneralAudioSource.Play();
            }
        }
    }

    public void FactionSelected()
    {
        if (SoundActive)
        {
            if (factionslected != null)
            {
                GeneralAudioSource.clip = factionslected;
                GeneralAudioSource.Play();
            }
        }
    }

    public void StopMenuMusic()
    {
        MusicAudioSource.Stop();
    }
}
