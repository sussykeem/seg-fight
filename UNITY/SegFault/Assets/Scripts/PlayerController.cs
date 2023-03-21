using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Jump.performed += ctx => Jump();
    }

    void Jump()
    {
        transform.Translate(Vector3.up);
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
