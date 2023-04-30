using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public GameObject[] playerPrefab = new GameObject[4];
    private PlayerController playerController;

    public int playerC;

    Vector2 startPos = new Vector2(0, 0);

    private void Awake()
    {
        playerC = PlayerInputManager.instance.playerCount;
        if(playerPrefab != null)
        {
            if(playerC > 1)
            {
                playerController = GameObject.Instantiate(playerPrefab[1], PlayerSpawnManageer.instance.spawnPoints[1].transform.position, transform.rotation).GetComponent<PlayerController>();
            } else {
                playerController = GameObject.Instantiate(playerPrefab[0], PlayerSpawnManageer.instance.spawnPoints[0].transform.position, transform.rotation).GetComponent<PlayerController>();
            }
            transform.parent = playerController.transform;
            transform.position = playerController.transform.position;
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        playerController.OnMove(context);
    }
}
