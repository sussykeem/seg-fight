using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public bool onGround = true;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3) //Checking if the player is on the ground
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
}
