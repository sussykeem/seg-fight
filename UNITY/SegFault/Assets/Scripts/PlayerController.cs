using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public PlayerControls controls;
    public Rigidbody2D rb;
    
    public Vector2 MoveDir = Vector2.zero;
    public float jumpPower = 40.0f;
    public float moveSpeed = 10.0f;
    public bool onGround = false;
    private void Awake()
    {
        controls = new PlayerControls();

    }
    private void Update()
    {
        MoveDir = controls.Gameplay.Movement.ReadValue<Vector2>();

        
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(MoveDir.x * moveSpeed, 0);
        if (onGround && MoveDir.y > 0)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 3) //Checking if the player is on the ground
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3) //Checking if the player is not on the ground
        {
            onGround = false;
        }
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
