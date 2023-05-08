using UnityEngine.Audio;
using System;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioManager audioManager;
    // Start is called before the first frame update
    public void button_sound()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("Button Press");
    }

    public void button_swoosh()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("Bloop");
    }

    public void EndMusic(string song)
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Stop(song);
    }

    public void SetFightMusic(string song)
    {
        Sound s = Array.Find(audioManager.sounds, sound => sound.name == "Fight Music");
        Sound newMusic = Array.Find(audioManager.sounds, sound => sound.name == song);
        s.source.clip = newMusic.clip;
    }
}
