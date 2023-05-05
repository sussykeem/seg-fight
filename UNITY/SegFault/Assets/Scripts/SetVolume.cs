using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    [SerializeField] private Slider volumeSlider = null;
    
    private void Start()
    {
        LoadVolume();
    }

    public void SetVolLevel(float sliderVal) 
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderVal) * 20);
        PlayerPrefs.SetFloat("Volume", sliderVal);
        LoadVolume();
    }

    void LoadVolume()
    {
        float volumeVal = PlayerPrefs.GetFloat("Volume");
        volumeSlider.value = volumeVal;
    }
}
