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
    public AudioSource MenuEffectsAudioSource;
    public AudioSource GameEffectsAudioSource;

    [Header("Clips Menu")]
    public AudioClip ButtonSelectionClip;
    public AudioClip VolumeBarClip;
    public AudioClip MagicFactionSelectionClip;
    public AudioClip ScienceFactionSelectionClip;
    public AudioClip StartDraftClip;
    public AudioClip SelectDraftPawnClip;

    [Header("Clip Game")]
    public AudioClip ActiveSuperAttackClip;
    public AudioClip TrapTileSafeClip;
    public AudioClip TrapTileClip;

    [Header("Clip Pawns")]
    public AudioClip MagicPawnDeath;
    public AudioClip SciencePawnDeath;

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
        {
            MusicAudioSource.Pause();
            GameEffectsAudioSource.Pause();
        }
    }

    private void OnGameUnPause()
    {
        if (SoundActive)
        {
            MusicAudioSource.Play();
            GameEffectsAudioSource.Play();
        }
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
            if (ButtonSelectionClip != null)
            {
                MenuEffectsAudioSource.clip = ButtonSelectionClip;
                MenuEffectsAudioSource.Play();
            }
        }
    }

    public void VolumeBar()
    {
        if (SoundActive)
        {
            if (VolumeBarClip != null)
            {
                MenuEffectsAudioSource.clip = VolumeBarClip;
                MenuEffectsAudioSource.Play();
            }
        }
    }

    public void StartDraft()
    {
        if (SoundActive)
        {
            if (StartDraftClip != null)
            {
                MenuEffectsAudioSource.clip = StartDraftClip;
                MenuEffectsAudioSource.Play();
            }
        }
    }

    public void SelectDraftPawn()
    {
        if (SoundActive)
        {
            if (SelectDraftPawnClip != null)
            {
                MenuEffectsAudioSource.clip = SelectDraftPawnClip;
                MenuEffectsAudioSource.Play();
            }
        }
    }

    public void FactionSelected(Factions faction)
    {
        if (SoundActive)
        {
            switch (faction)
            {
                case Factions.Magic:
                    if (MagicFactionSelectionClip != null)
                    {
                        MenuEffectsAudioSource.clip = MagicFactionSelectionClip;
                        MenuEffectsAudioSource.Play();
                    }
                    break;
                case Factions.Science:
                    if (ScienceFactionSelectionClip != null)
                    {
                        MenuEffectsAudioSource.clip = ScienceFactionSelectionClip;
                        MenuEffectsAudioSource.Play();
                    }
                    break;
            }
        }
    }

    public void ActiveSuperAttack()
    {
        if (SoundActive)
        {
            if (ActiveSuperAttackClip != null)
            {
                GameEffectsAudioSource.clip = ActiveSuperAttackClip;
                GameEffectsAudioSource.Play();
            }
        }
    }

    public void TrapTile(bool active)
    {
        if (SoundActive)
        {
            if (active)
            {
                if (TrapTileClip != null)
                {
                    GameEffectsAudioSource.clip = TrapTileClip;
                    GameEffectsAudioSource.Play();
                }
            }
            else
            {
                if (TrapTileSafeClip != null)
                {
                    GameEffectsAudioSource.clip = TrapTileSafeClip;
                    GameEffectsAudioSource.Play();
                }
            }
        }
    }

    public void PawnSFX(AudioClip ClipToPlay)
    {
        if (SoundActive)
        {
            if (ClipToPlay != null)
            {
                GameEffectsAudioSource.clip = ClipToPlay;
                GameEffectsAudioSource.Play();
            }
        }
    }

    public void PawnDeathVFX(Factions faction)
    {
        if (SoundActive)
        {
            switch (faction)
            {
                case Factions.Magic:
                    if (MagicPawnDeath != null)
                    {
                        GameEffectsAudioSource.clip = MagicPawnDeath;
                        GameEffectsAudioSource.Play();
                    }
                    break;
                case Factions.Science:
                    if (SciencePawnDeath != null)
                    {
                        GameEffectsAudioSource.clip = SciencePawnDeath;
                        GameEffectsAudioSource.Play();
                    }
                    break;
            }
        }
    }
}
