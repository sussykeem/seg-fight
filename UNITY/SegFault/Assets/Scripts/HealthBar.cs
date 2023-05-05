using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public TextMeshProUGUI healthBox;
    public float healthAmount = 100.0f;
    public float health = 0;
    public GameObject[] players;
    public GameObject player;
    private PlayerController playerController;
    
    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (gameObject.name[0] == '1')
        {
            player = players[0];
        } else if (gameObject.name[0] == '2')
        {
            player = players[1];
        }
        playerController = player.GetComponent<PlayerController>();
    }
    private void FixedUpdate()
    {
        health = playerController.health;
        healthBar.fillAmount = health/healthAmount;
        if(health <= 0)
        {
            health = 0;
        }
        healthBox.text = health.ToString();
    }
}
