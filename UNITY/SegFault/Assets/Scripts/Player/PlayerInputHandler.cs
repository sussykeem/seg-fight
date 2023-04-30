using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public GameObject[] playerPrefab = new GameObject[4];
    private GameObject playerOb;
    private PlayerController playerController;

    public int playerC;
    public int player = 0;
    public int rotPlayer = 0;

    Vector2 spawnPos = Vector2.zero;
    Vector3 eulerRot = Vector3.zero;
    Quaternion fixedQuat; 

    private void Awake()
    {
        playerC = PlayerInputManager.instance.playerCount;
        if(playerPrefab != null)
        {
            if(playerC > 1)
            { //Player 2
                playerOb = playerPrefab[player];
                eulerRot = playerPrefab[player].transform.rotation.eulerAngles;
                rotPlayer = 180;
                spawnPos = PlayerSpawnManageer.instance.spawnPoints[1].transform.position;
            } else { //Player 1
                playerOb = playerPrefab[player];
                eulerRot = playerPrefab[player].transform.rotation.eulerAngles;
                spawnPos = PlayerSpawnManageer.instance.spawnPoints[0].transform.position;
            }
            fixedQuat = Quaternion.Euler(eulerRot.x, eulerRot.y + rotPlayer, eulerRot.z);
            playerController = GameObject.Instantiate(playerOb, spawnPos, fixedQuat).GetComponent<PlayerController>();
            transform.parent = playerController.transform;
            transform.position = playerController.transform.position;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerController.OnMove(context);
    }
}
