// Karim Chmayssani
// simple player movement script that initializes some components, runs an update function
// to move the player and updates the animation playing based on direction

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    public Animator animator;

    Vector2 movement;

    bool movingHor, movingVert;

    // Update is called once per frame
    void Update() {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal"); // gets the x and y input
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0) { // get bool data about which dir we are moving
            movingHor = true;
        } else {
            movingHor = false;
        }

        if (movement.y != 0) {
            movingVert = true;
        } else {
            movingVert = false;
        }

        animator.SetFloat("Horizontal", movement.x); // update the animator bools to choose which one to play
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetBool("movingHor", movingHor);
        animator.SetBool("movingVert", movingVert);
    }
    void FixedUpdate() {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime); // fixed update makes movement
        // not reliant on framerate, making it more consistent
    }
}