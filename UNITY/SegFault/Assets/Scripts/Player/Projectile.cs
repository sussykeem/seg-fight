using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject[] players;
    private GameObject thisPlayer;
    private PlayerController playerController;
    public int attackPower = 0;
    public int attInd = 0;
    private float projSpeed = 15f;
    private int despawnPos = 20;
    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (gameObject.name[0] == 'B')
        { //Blake's projectile
            if (players[0].name[0] == 'B')
            {
                thisPlayer = players[0];
            } else
            {
                thisPlayer = players[1];
            }
        } else
        { //Randy's projectile
            if (players[0].name[0] == 'R')
            {
                thisPlayer = players[0];
            } else
            {
                thisPlayer = players[1];
            }
        }
        playerController = thisPlayer.GetComponent<PlayerController>();
        attackPower = playerController.attackPower;
        attInd = playerController.attInd;
    }

    private void FixedUpdate()
    {
        if(transform.position.x <= -despawnPos  || transform.position.x >= despawnPos)
        {
            Destroy(gameObject);
        }
        transform.Translate(new Vector2(1 * projSpeed * Time.deltaTime, 0));
    }
}
