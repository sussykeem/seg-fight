using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public GameObject[] playerPrefab = new GameObject[4];
    private GameObject playerOb;
    private PlayerController playerController;

    private GameObject pauseManager;
    private Pause pauseSc;

    public int playerC;
    public int player1 = 0;
    public int player2 = 0;
    public int rotPlayer = 0;

    Vector2 spawnPos = Vector2.zero;
    Vector3 eulerRot = Vector3.zero;
    Quaternion fixedQuat;

    private void Awake()
    {
        pauseManager = GameObject.FindGameObjectWithTag("Pause");
        pauseSc = pauseManager.GetComponent<Pause>();

        playerC = PlayerInputManager.instance.playerCount;
        player1 = PlayerPrefs.GetInt("player1");
        player2 = PlayerPrefs.GetInt("player2");
        if (playerPrefab != null)
        {
            if (playerC > 1)
            { //Player 2
                playerOb = playerPrefab[player2];
                eulerRot = playerPrefab[player2].transform.rotation.eulerAngles;
                rotPlayer = 180;
                spawnPos = PlayerSpawnManageer.instance.spawnPoints[1].transform.position;
            }
            else
            { //Player 1
                playerOb = playerPrefab[player1];
                eulerRot = playerPrefab[player1].transform.rotation.eulerAngles;
                spawnPos = PlayerSpawnManageer.instance.spawnPoints[0].transform.position;
            }
            fixedQuat = Quaternion.Euler(eulerRot.x, eulerRot.y + rotPlayer, eulerRot.z);
            playerController = GameObject.Instantiate(playerOb, spawnPos, fixedQuat).GetComponent<PlayerController>();
            transform.parent = playerController.transform;
            transform.position = playerController.transform.position;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    { //Handles player move
        playerController.OnMove(context);
    }

    public void OnLAtt(InputAction.CallbackContext context)
    { //Handles player light attack
        playerController.OnLAtt(context);
    }

    public void OnHAtt(InputAction.CallbackContext context)
    { //Handles player heavy attack
        playerController.OnHAtt(context);
    }

    public void OnShAtt(InputAction.CallbackContext context)
    { //Handles player shield break
        playerController.OnShAtt(context);
    }

    public void OnSpAtt(InputAction.CallbackContext context)
    { //Handles player special attack
        playerController.OnSpAtt(context);
    }

    public void OnPause(InputAction.CallbackContext context)
    { //Pause the game
        if (context.performed)
        {
            pauseSc.onPause();
        }
    }
}

