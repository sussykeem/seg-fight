using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Round : MonoBehaviour
{
    public TextMeshProUGUI roundBox;
    private GameObject [] players;
    private GameObject player1, player2;
    private PlayerController playerController1, playerController2;

    public GameObject playerManager;
    private PlayerSpawnManageer playerManagerSc;

    private GameOver gameOverSc;

    public GameObject timerObj;
    private Timer timerSc;
    private float timer = 0.0f; 

    private GameObject[] spawnPoints;

    public int roundNum = 1;
    private string roundString;

    public void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        player1 = players[0];
        player2 = players[1];
        playerController1 = player1.GetComponent<PlayerController>();
        playerController2 = player2.GetComponent<PlayerController>();

        playerManagerSc = playerManager.GetComponent<PlayerSpawnManageer>();

        spawnPoints = playerManagerSc.spawnPoints;

        timerSc = timerObj.GetComponent<Timer>();

        gameOverSc = gameObject.GetComponent<GameOver>();
    }

    public void FixedUpdate()
    {
        timer = timerSc.getTime();
        if (timer <= 0.0f)
        { //Time ends, change rounds
            roundChange();
        }
        else if (playerController1.numWins >= 2 || playerController2.numWins >= 2)
        { //Either player has completely won the game
            if(playerController1.numWins >= 2)
            {
                gameWon(player1);
            } else
            {
                gameWon(player2);
            }
        }
        else if (playerController1.playerWon || playerController2.playerWon)
        { //A player has won the round
            roundChange();
        }
}
    public void roundChange()
    {
        roundNum++;
        switch (roundNum)
        {
            case 1:
                roundString = "I";
                break;
            case 2:
                roundString = "II";
                break;
            case 3:
                roundString = "III";
                break;
            case 4:
                gameTie();
                break;
            default:
                roundString = "I";
                break;
        }
        roundBox.text = roundString;
        playerController1.health = 100f;
        playerController2.health = 100f;
        playerController1.playerWon = false;
        playerController2.playerWon = false;
        player1.transform.position = spawnPoints[0].transform.position;
        player2.transform.position = spawnPoints[1].transform.position;
        timerSc.timer = timerSc.roundTime;
    }

    public void gameWon(GameObject player)
    { //A player has won the game
        gameOverSc.onGameOver(player);
    }

    public void gameTie()
    { //If the game ended in a tie
        gameOverSc.onGameTie();
    }
}
