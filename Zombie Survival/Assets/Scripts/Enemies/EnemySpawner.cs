using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool enemiesDead = true;
    public bool enemiesSpawning = false;
    public bool finishedSpawning = true;

    public int enemiesToSpawn = 8;

    public GameObject spawnParent;
    public Transform[] spawnPoints;
    public GameObject enemyParent; 

    public void Start()
    {
        spawnParent = GameObject.Find("Spawnpoints");
        spawnPoints = spawnParent.GetComponentsInChildren<Transform>();
        enemyParent = Resources.Load("Prefabs/Enemy") as GameObject;
    }

    public void Update()
    {
        if (enemiesDead == true && finishedSpawning == true)
        {
            enemiesSpawning = true;
            enemiesDead = false;
            finishedSpawning = false;
        }

        Debug.Log(enemiesToSpawn / spawnPoints.Length);
        if (enemiesSpawning == true)
        {
            for (int i = 0; i <= enemiesToSpawn / spawnPoints.Length; i++)
            {
                for (int j = 0; j < spawnPoints.Length; j++)
                {
                    Spawn(j);
                }

            }
        }
    }

    public void Spawn(int spawnIndex)
    {
        GameObject enemy = Instantiate(enemyParent, spawnPoints[spawnIndex]);
    }
}
