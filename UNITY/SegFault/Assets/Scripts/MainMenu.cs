using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor; // include to quit in test

public class MainMenu : MonoBehaviour
{
   public void play_game()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    
   public void quit_game()
    {
        EditorApplication.isPlaying = false; // so it quits in test
        Application.Quit();
    }
    
}
