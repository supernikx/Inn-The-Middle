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
                resolutionOptions.Add(r.width + " x "+r.height);
            }
            ResolutionDropdown.AddOptions(resolutionOptions);
        }
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    private void LoadSettings()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", -50f);
        EffectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume", -50f);
        audioMixer.SetFloat("MusicVolume", MusicSlider.value);
        audioMixer.SetFloat("SFXVolume", EffectsSlider.value);
        currentResolutionIndex = PlayerPrefs.GetInt("Resolution", 0);
        Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, true);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
        QualityDropdown.value = PlayerPrefs.GetInt("Quality", 2);
        QualityDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        currentResolutionIndex = resolutionIndex;
    }

    public void SetQuality(int _qualityIndex)
    {
        qualityIndex = _qualityIndex;
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        EffectsVolume = volume;
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void Apply()
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, true);
        SaveSettings();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("EffectsVolume", EffectsVolume);
        PlayerPrefs.SetInt("Resolution", currentResolutionIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
        Debug.Log("Impostazioni salvate");
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
