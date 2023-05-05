using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackScri : MonoBehaviour
{
    private PlayerController playerController;
    private BoxCollider2D thisBC;

    public bool yesAttack;
    public int attackPower;

    private void OnEnable()
    {
        yesAttack = true;
    }
    private void Awake()
    {
        playerController = gameObject.GetComponentInParent<PlayerController>(); 
        thisBC = gameObject.GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (thisBC.isActiveAndEnabled)
        {
            yesAttack = true;
        } else
        {
            yesAttack = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == playerController.otherPlayer.name && yesAttack)
        { //The collision was with the other player
            //playerController.otherPlayer.GetComponent<PlayerController>().OnHit(playerController.attackPower);
        }
    }
}
