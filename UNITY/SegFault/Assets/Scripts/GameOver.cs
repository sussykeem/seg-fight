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
    private bool gameOver = false;
    private float timeDeath = 1f;
    private GameObject thisPlayer;
    public void onGameOver(GameObject player)
    {
        thisPlayer = player;
        player.GetComponent<PlayerController>().otherPlayer.GetComponent<PlayerController>().anim.SetBool("dead", true);
        playerName = player.name.Substring(0, player.name.Length - 7);
        gameOverTxt.text = playerName + " Wins";
        gameOver = true;
    }

    private void FixedUpdate()
    {
        if (gameOver)
        {
            timeDeath -= Time.deltaTime;
            Debug.Log(timeDeath);
        }
        if (gameOver && timeDeath <= 0)
        {
            Debug.Log("He Dead girl");
            thisPlayer.GetComponent<PlayerController>().otherPlayer.GetComponent<PlayerController>().anim.SetBool("dead", true);
            Destroy(thisPlayer.GetComponent<PlayerController>().otherPlayer);
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);
        }   
    }

    public void onGameTie()
    {
        gameOverTxt.text = "Tie";
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }
    public void onHome(int sceneId)
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.Stop("Fight Music");
        audioManager.Play("Menu Music");
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneId);
    }
}
