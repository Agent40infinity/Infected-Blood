using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 *------------------------------------------------------------------------*/

public class EnemySpawner : MonoBehaviour
{
    //General:
    public bool enemiesSpawning = false;
    public bool finishedSpawning = true;
    public int enemiesToSpawn = 8;

    //References:
    public GameObject spawnParent;
    public Transform[] spawnPoints;
    public GameObject enemyParent;

    public void Start()
    {
        spawnParent = GameObject.Find("Spawnpoints");
        spawnPoints = spawnParent.GetComponentsInChildren<Transform>();
        enemyParent = Resources.Load("Prefabs/Enemy") as GameObject;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Debug.Log(spawnPoints[i].position);
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            Destroy(GameObject.FindWithTag("Enemy"));
        }

        if (enemiesSpawning == true) //Checks whether or not enemies are needed to be spawned
        {
            int sT = 0;
            for (int i = 1; i < spawnPoints.Length; i++) //Used to cycle through spawnpoints
            { 
                StartCoroutine(Spawn(i, sT)); //Spawns an enemy
                GameManager.enemiesAlive++;
                enemiesToSpawn--; //Decreases the enemiesToSpawn counter
                sT += 3; //Adds to the existing spawn rate timer
                if (enemiesToSpawn > 0 && i == spawnPoints.Length - 1) //Checks whether or not enemies are still needed to be spawned and if the spawnpoint's array has reached the final spawnpoint to allow for the spawning to loop
                {
                    i = 0; //resets the loop
                }
                else if (enemiesToSpawn == 0) //If the enemiesToSpawn count becomes 0, ends enemy spawning
                {
                    i = spawnPoints.Length; //ends the loop
                    enemiesSpawning = false; //Ceases the ability to spawn more enemies
                    finishedSpawning = true; 
                }
            }
        }
    }

    IEnumerator Spawn(int spawnIndex, int spawnTime) //Used to spawn enemies
    {
        yield return new WaitForSeconds(spawnTime); //Determines the amount of time between each set spawn
        GameObject enemy = Instantiate(enemyParent, spawnPoints[spawnIndex].position, Quaternion.identity, spawnPoints[spawnIndex]); //Instantiates a new enemy based on the spawnpoint arry's index
    }
}
