using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public bool onGround = true;
    public float repelFactor = 1f;
    private Rigidbody2D prb;
    private Transform ptr;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3) //Checking if the player is on the ground
        {
            onGround = true;
        }
        else if (collision.gameObject.tag == "Player" && onGround == false)
        { // this is so a player cannot get stuck on top of another play, it lightly forces the players off eachother
            ptr = transform.parent;
            prb = ptr.GetComponent<Rigidbody2D>();
            var repelDir = -1 * (collision.gameObject.transform.position - ptr.position).normalized.x;
            prb.AddForce(new Vector2(repelFactor * repelDir, 1 * repelFactor), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3) //Checking if the player is not on the ground
        {
            onGround = false;
        }
    }
}
