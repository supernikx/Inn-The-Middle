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
                else if (!SoundActive)
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

    bool SFXWasPlaying = false;
    private void OnGamePause()
    {
        if (SoundActive)
        {
            MusicAudioSource.Pause();
            if (GameEffectsAudioSource.isPlaying)
            {
                SFXWasPlaying = true;
                GameEffectsAudioSource.Pause();
            }
            else
            {
                SFXWasPlaying = false;
            }
        }
    }

    private void OnGameUnPause()
    {
        if (SoundActive)
        {
            MusicAudioSource.Play();
            if (SFXWasPlaying)
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

    /// <summary>
    /// Funzione che attiva/disattiva i suoni
    /// </summary>
    public void ActiveDeactiveSound()
    {
        SoundActive = !SoundActive;
    }

    /// <summary>
    /// Funzione che esegue il suono ButtonSelection
    /// </summary>
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

    /// <summary>
    /// Funzione che esegue il suono VolumeBar
    /// </summary>
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

    /// <summary>
    /// Funzione che esegue il suono StartDraft
    /// </summary>
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

    /// <summary>
    /// Funzione che esegue il suono SelectDraftPawn
    /// </summary>
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

    /// <summary>
    /// Funzione che esegue il suono della fazione selezionata (passata come parametro)
    /// </summary>
    /// <param name="faction"></param>
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

    /// <summary>
    /// Funzione che esegue il suono di attivazione/disattivazione superattacco
    /// </summary>
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

    /// <summary>
    /// Funzione che esegue il suono di attivazione/disattivazione di una casella trappola
    /// </summary>
    /// <param name="active"></param>
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

    /// <summary>
    /// Funzione che esegue l'SFX di una pedina passandogli la clip come parametro
    /// </summary>
    /// <param name="ClipToPlay"></param>
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

    /// <summary>
    /// Funzione che esegue il suono di morte della pedina passandogli la fazione come parametro
    /// </summary>
    /// <param name="faction"></param>
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

    /// <summary>
    /// Funzione che e stoppa il suono di movimento della pedina una volta finito
    /// </summary>
    public void PawnMovementEnd()
    {
        if (GameEffectsAudioSource.isPlaying)
            GameEffectsAudioSource.Stop();
    }
}
