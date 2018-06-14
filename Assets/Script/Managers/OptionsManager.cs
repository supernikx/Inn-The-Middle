using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

[System.Serializable]
public class Resolutions
{
    public int width;
    public int height;
}

[System.Serializable]
public class OptionsManager : MonoBehaviour
{
    //public GameObject StartMainMenuButton;

    [Header("Sound Settings")]
    public AudioMixer audioMixer;
    public Slider MusicSlider;
    public Slider EffectsSlider;
    float MusicVolume;
    float EffectsVolume;

    [Header("Screen Settings")]
    public Dropdown ResolutionDropdown;
    public Dropdown QualityDropdown;
    public Toggle FullScreenToggle;
    public List<Resolutions> resolutions = new List<Resolutions>();
    List<string> resolutionOptions = new List<string>();
    int currentResolutionIndex;
    int qualityIndex;

    private void Start()
    {
        LoadResolutions();
        LoadSettings();
        gameObject.SetActive(false);
    }

    private void LoadResolutions()
    {
        if (resolutions.Count > 0)
        {
            ResolutionDropdown.ClearOptions();
            foreach (Resolutions r in resolutions)
            {
                resolutionOptions.Add(r.width + " x " + r.height);
            }
            ResolutionDropdown.AddOptions(resolutionOptions);
        }
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    private void LoadSettings()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", -40f);
        EffectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume", -40f);
        audioMixer.SetFloat("MusicVolume", MusicSlider.value);
        audioMixer.SetFloat("SFXVolume", EffectsSlider.value);
        if (PlayerPrefs.GetInt("FullScreen", 1) == 1)
        {
            Screen.fullScreen = true;
            FullScreenToggle.isOn = true;
        }
        else
        {
            Screen.fullScreen = false;
            FullScreenToggle.isOn = false;
        }
        currentResolutionIndex = PlayerPrefs.GetInt("Resolution", 0);
        Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, Screen.fullScreen);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
        QualityDropdown.value = PlayerPrefs.GetInt("Quality", 2);
        QualityDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        currentResolutionIndex = resolutionIndex;
        Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", currentResolutionIndex);
    }

    public void SetFullScreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        if (fullscreen)
        {
            PlayerPrefs.SetInt("FullScreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("FullScreen", 0);
        }
    }

    public void SetQuality(int _qualityIndex)
    {
        qualityIndex = _qualityIndex;
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        EffectsVolume = volume;
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("EffectsVolume", EffectsVolume);
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("EffectsVolume", EffectsVolume);
        PlayerPrefs.SetInt("Resolution", currentResolutionIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
        if (Screen.fullScreen)
        {
            PlayerPrefs.SetInt("FullScreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("FullScreen", 0);
        }
        Debug.Log("Impostazioni salvate");
    }

    public void ResetToDefault()
    {
        FullScreenToggle.isOn = true;
        qualityIndex = 2;
        QualityDropdown.value = qualityIndex;
        QualityDropdown.RefreshShownValue();
        QualitySettings.SetQualityLevel(qualityIndex);
        currentResolutionIndex = 0;
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
        Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, true);
        audioMixer.SetFloat("MusicVolume", -40f);
        audioMixer.SetFloat("SFXVolume", -40f);
        MusicSlider.value = -40f;
        EffectsSlider.value = -40f;
        SaveSettings();
    }

    #region FocusFix

    public void SetOptionsFocus()
    {
        StartCoroutine(SetOptionsCoroutine());
    }

    private IEnumerator SetOptionsCoroutine()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(ResolutionDropdown.gameObject);
    }

    public void DisableFocus()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    #endregion
}
