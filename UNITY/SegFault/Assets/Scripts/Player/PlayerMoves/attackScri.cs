using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackScri : MonoBehaviour
{
    private PlayerController playerController;
    private Dictionary<int, int[]>[] moveContainer;
    private int[] damFrames;
    private bool[] attackType;
    private BoxCollider2D[] damageColliders;
    public BoxCollider2D lCol, hCol, spCol, shCol;

    public bool yesAttack;
    public int attackPower; 
    private void OnEnable()
    {
        yesAttack = true;
    }
    private void Awake()
    {
        playerController = gameObject.GetComponentInParent<PlayerController>(); 
    }

    private void Start()
    {
        moveContainer = playerController.moveContainer;
        damageColliders = playerController.damageColliders;
        lCol = damageColliders[0];
        hCol = damageColliders[1];
        spCol = damageColliders[2];
        shCol = damageColliders[3];
    }

    public void FixedUpdate()
    {
        attackType = playerController.attackType;    
    }

    public void attack()
    {
        for (int i = 0; i < attackType.Length; i++)
        {
            if (attackType[i] == true){
                damageColliders[i].enabled = true;
                foreach (var testValue in moveContainer[i])
                {
                    attackPower = testValue.Key;
                    damFrames = testValue.Value;
                }
            } else if (attackType[i] == false) {
                damageColliders[i].enabled = false;
            }
        }

    }
}
