using UnityEngine.Audio;
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
}
