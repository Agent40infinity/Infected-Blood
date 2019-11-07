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

    //Tests
    public RoomData[] roomParents;
    public List<Transform> activeSpawners = new List<Transform>();

    public struct RoomData
    {
        public GameObject room;
        public Transform[] spawnPoints;
    }

    public void Start()
    {
        spawnParent = GameObject.Find("Spawnpoints");
        spawnPoints = spawnParent.GetComponentsInChildren<Transform>();
        enemyParent = Resources.Load("Prefabs/Enemy") as GameObject;

        roomParents = new RoomData[GameObject.FindGameObjectsWithTag("SpawnRooms").Length];
        for (int i = 0; i < roomParents.Length; i++)
        {
            roomParents[i].room = GameObject.FindGameObjectsWithTag("SpawnRooms")[i];
            roomParents[i].spawnPoints = roomParents[i].room.GetComponentsInChildren<Transform>();
            for (int x = 0; x < roomParents[i].spawnPoints.Length; x++)
            {
                Debug.Log(roomParents[i].spawnPoints[x]);
            }
        }

        UnlockRoom(0);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            Destroy(GameObject.FindWithTag("Enemy"));
        }

        if (enemiesSpawning == true) //Checks whether or not enemies are needed to be spawned
        {
            int sT = 0; //25
            int spawnCap = 0;
            for (int i = 0; i < activeSpawners.Count; i++) //Used to cycle through spawnpoints
            {
                if (GameManager.enemiesAlive <= 100)
                {
                    StartCoroutine(Spawn(i, sT, spawnCap)); //Spawns an enemy
                    spawnCap++;
                    enemiesToSpawn--; //Decreases the enemiesToSpawn counter

                    if (spawnCap == activeSpawners.Count)
                    {
                        sT += 4; //Adds to the existing spawn rate timer
                        spawnCap = 0;
                    }
                    if (enemiesToSpawn > 0 && i == activeSpawners.Count - 1) //Checks whether or not enemies are still needed to be spawned and if the spawnpoint's array has reached the final spawnpoint to allow for the spawning to loop
                    {
                        i = -1; //resets the loop
                    }
                    else if (enemiesToSpawn == 0) //If the enemiesToSpawn count becomes 0, ends enemy spawning
                    {
                        i = activeSpawners.Count; //ends the loop
                        enemiesSpawning = false; //Ceases the ability to spawn more enemies
                        finishedSpawning = true;
                    }
                }
                else
                {
                    i--;
                }
            }
        }
    }

    public void UnlockRoom(int index)
    {
        for (int i = 1; i < roomParents[index].spawnPoints.Length; i++)
        {
            activeSpawners.Add(roomParents[index].spawnPoints[i]);
        }
    }

    IEnumerator Spawn(int spawnIndex, int spawnTime, int spawnCap) //Used to spawn enemies
    {
        yield return new WaitForSeconds(spawnTime); //Determines the amount of time between each set spawn
        GameManager.enemiesAlive++;
        GameObject enemy = Instantiate(enemyParent, activeSpawners[spawnIndex].position, Quaternion.identity, activeSpawners[spawnIndex]); //Instantiates a new enemy based on the spawnpoint arry's index
    }
}
