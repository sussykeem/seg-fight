using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI gameOverTxt;
    public GameObject gameOverPanel;
    private string playerName;
    public void onGameOver(GameObject player)
    {
        playerName = player.name.Substring(0, player.name.Length - 7);
        gameOverTxt.text = playerName + " Wins";
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void onGameTie()
    {
        gameOverTxt.text = "Tie";
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }
    public void onHome(int sceneId)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneId);
    }
}
