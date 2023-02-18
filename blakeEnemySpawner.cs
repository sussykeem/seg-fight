/*
 * Blake Milstead
 * This is a simple spawn manager that would spawn enemies
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab

    public float spawnXMin = -10; // set min spawn X for stars
    public float spawnXMax = 15; // set max spawn X for stars
    public float spawnYMin = -5;
    public float spawnYMax = 10;

    public float speedEnemy = 5.0f;
    public float enemyNum = 3.0f;

    void Awake()
    {
        SpawnEnemyStars(); //spawns initial Enemies
    }

    void Update()
    {

        SpawnEnemy();
        
    }


    void SpawnBackStars() //Spawns background stars
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < enemyNum) // if there are less Enemies on the screen than enemyNum make more
        {
            xPos = Random.Range(spawnStXMin, spawnStXMax);
            yPos = Random.Range(spawnYMin, spawnYMax);
            Vector2 initialEnemyPos = new Vector2(xPos, yPos);
            Instantiate(enemyPrefab, initialEnemyPos, enemyPrefab.transform.rotation);
        }
    }

}
