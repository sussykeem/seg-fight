using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitCheck : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = gameObject.GetComponentInParent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MoveHit")
        { //The collision was with the other player's hitbox
            playerController.GetComponent<PlayerController>().OnHit(playerController.otherPlayer.GetComponent<PlayerController>().attackPower);
        }
    }
}
