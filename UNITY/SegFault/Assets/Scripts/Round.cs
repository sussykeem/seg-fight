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

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
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
            default:
                roundString = "I";
                break;
        }
        roundBox.text = roundString;
        playerController1.health = 100f;
        playerController2.health = 100f;
        player1.transform.position = spawnPoints[0].transform.position;
        player2.transform.position = spawnPoints[0].transform.position;
    }
}
