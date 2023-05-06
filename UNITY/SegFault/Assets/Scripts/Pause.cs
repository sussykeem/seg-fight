using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;

    public void onPause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void onResume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void onHome(int sceneId)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneId);
    }
}
