using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeScript : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    private const string masterVolKey = "preferredMasterVolume";
    private const string musicVolKey = "preferredMusicVolume";
    private const string sfxVolKey = "preferredSFXVolume";

    private void Start() {
        if (PlayerPrefs.HasKey(masterVolKey)) {
            masterSlider.value = PlayerPrefs.GetFloat(masterVolKey);
        }
        if (PlayerPrefs.HasKey(musicVolKey)) {
            musicSlider.value = PlayerPrefs.GetFloat(musicVolKey);
        }
        if (PlayerPrefs.HasKey(sfxVolKey)) {
            sfxSlider.value = PlayerPrefs.GetFloat(sfxVolKey);
        }
    }

    public void setMasterVolume(float sliderVal) {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderVal)*20);
        PlayerPrefs.SetFloat(masterVolKey, sliderVal);
    }

    public void setMusicVolume(float sliderVal) {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderVal) * 20);
        PlayerPrefs.SetFloat(musicVolKey, sliderVal);
    }

    public void setSFXVolume(float sliderVal) {
        mixer.SetFloat("SFXVolume", Mathf.Log10(sliderVal) * 20);
        PlayerPrefs.SetFloat(sfxVolKey, sliderVal);
    }
    
    public float GetVolume(string key) {
        bool result = mixer.GetFloat(key, out float value);
        if (result == true) {
            return value;
        }
        else {
            return -40f;
        }
    }
}
