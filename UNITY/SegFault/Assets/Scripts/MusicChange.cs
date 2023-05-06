using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class MusicChange : MonoBehaviour
{
    [HideInInspector] public AudioManager audioManager;
    public string FightMusic, Gong;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play(Gong);
        audioManager.Play(FightMusic);
    }
}
