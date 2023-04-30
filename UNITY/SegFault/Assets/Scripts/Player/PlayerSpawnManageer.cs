using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManageer : MonoBehaviour
{
    public GameObject[] spawnPoints = new GameObject[2];
    public static PlayerSpawnManageer instance = null;
    private Gamepad pairWithDevice;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        var gamePads = Gamepad.all;
        PlayerInputManager.instance.JoinPlayer(0, -1, controlScheme: "Gamepad", pairWithDevice: gamePads[0]);
        PlayerInputManager.instance.JoinPlayer(1, -1, controlScheme: "Keyboard", pairWithDevice: Keyboard.current);
    }
}
