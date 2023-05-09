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
        if (collision.gameObject.tag == "MoveHit" && !playerController.isHit)
        { //The collision was with the other player's hitbox
            playerController.GetComponent<PlayerController>().OnHit(playerController.otherPlayer.GetComponent<PlayerController>().attackPower, playerController.otherPlayer.GetComponent<PlayerController>().attInd);
        } else if (collision.gameObject.tag == "Projectile" && collision.gameObject.name[0] != transform.parent.name[0] && !playerController.isHit)
        { //The collision was with a projectile that my character did not create
            playerController.GetComponent<PlayerController>().OnHit(collision.gameObject.GetComponent<Projectile>().attackPower, collision.gameObject.GetComponent<Projectile>().attInd);
            Destroy(collision.gameObject);
        }
    }
}
