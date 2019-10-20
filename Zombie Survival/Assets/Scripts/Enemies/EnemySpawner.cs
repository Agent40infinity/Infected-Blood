using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool enemiesDead = true;
    public bool enemiesSpawning = false;
    public bool finishedSpawning = true;

    public int enemiesToSpawn = 8;
    public int round = 0;

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
        if (enemiesDead == true && finishedSpawning == true)
        {
            round++;
            enemiesSpawning = true;
            enemiesDead = false;
            finishedSpawning = false;
        }

        if (enemiesSpawning == true)
        {
            int sT = 0;
            for (int i = 1; i < spawnPoints.Length; i++)
            { 
                StartCoroutine(Spawn(i, sT));
                enemiesToSpawn--;
                sT += 3;
                if (enemiesToSpawn > 0 && i == spawnPoints.Length - 1)
                {
                    i = 0;
                }
                else if (enemiesToSpawn == 0)
                {
                    i = spawnPoints.Length;
                    enemiesSpawning = false;
                    finishedSpawning = true;
                }
            }
        }
    }

    IEnumerator Spawn(int spawnIndex, int spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);
        GameObject enemy = Instantiate(enemyParent, spawnPoints[spawnIndex].position, Quaternion.identity, spawnPoints[spawnIndex]);
    }
}
