using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    public AudioMixer mixer;

    void Awake ()
    {
        if (instance == null)
            instance = this;
        else 
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        AudioMixerGroup[] audioMixerGroup = mixer.FindMatchingGroups("Master");
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            if (s.name == "Menu Music" || s.name == "Fight Music") s.source.outputAudioMixerGroup = audioMixerGroup[1];
            else s.source.outputAudioMixerGroup = audioMixerGroup[0];

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            
        }
    }

    void Start() 
    {
        float MasterVolume = PlayerPrefs.GetFloat("MasterVolume");
        mixer.SetFloat("MasterVolume", Mathf.Log10(MasterVolume) * 20 );
        AudioListener.volume = MasterVolume;
        float MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        mixer.SetFloat("MusicVolume", Mathf.Log10(MusicVolume) * 20);
        Play(sounds[0].name);
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (!(s.source.isPlaying)) {
            s.source.Play();
        }
        else
        {
            s.source.Stop();
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
}
