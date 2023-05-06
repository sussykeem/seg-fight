using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    [SerializeField] private Slider volumeSlider = null;
    public string AudioGroup;
    
    private void Start()
    {
        LoadVolume(AudioGroup);
    }

    public void SetVolLevel(float sliderVal) 
    {
        mixer.SetFloat(AudioGroup, Mathf.Log10(sliderVal) * 20);
        PlayerPrefs.SetFloat(AudioGroup, sliderVal);
        LoadVolume(AudioGroup);
    }

    void LoadVolume(string AudioGroup)
    {
        float volumeVal = PlayerPrefs.GetFloat(AudioGroup);
        volumeSlider.value = volumeVal;
       // AudioListener.volume = volumeVal;
    }
}
