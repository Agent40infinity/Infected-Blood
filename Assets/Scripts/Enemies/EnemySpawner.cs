using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 *------------------------------------------------------------------------*/

public class EnemySpawner : NetworkBehaviour
{
    #region Variables
    //General:
    public bool enemiesSpawning = false; //Checks whether or not the enemies are spawning
    public bool spawningPaused = false; //Checks whether or not the spawning has been paused
    public bool finishedSpawning = true; //Checks whether or not spawning has finished
    public int pausedIndex = 0; //Stores the index value of the spawner if spawning is paused
    public int enemiesToSpawn = 8; //Value for how many enemies need to be spawned
    public int maxEnemies = 100; //Value for the max amount of enemies that can be alive at the same time

    //References:
    public GameObject enemyParent; //Reference for the enemy prefab to allow for the enemies to spawn
    public RoomData[] roomParents; //Array of RoomData's to store all spawner locations based upon the room
    public List<Transform> activeSpawners = new List<Transform>(); //Creates a list to reference the active spawners 

    public struct RoomData //RoomData is used to store the room parent information and the spawnpoints attached to each room
    {
        public GameObject room; //Used to reference the room parent gameObject
        public Transform[] spawnPoints; //Array of Transforms to reference each spawnpoint within the room
    }
    #endregion

    #region General
    public void Start()
    {
        if (isServer)
        {
            enemyParent = Resources.Load("Prefabs/Enemy") as GameObject; //Loads the enemy prefab from file

            roomParents = new RoomData[GameObject.FindGameObjectsWithTag("SpawnRooms").Length]; //Sets the length of RoomParents to the length of returned array of GameObjects with the tag "SpawnRooms"
            for (int i = 0; i < roomParents.Length; i++) //Loop used to set up the RoomData values per entry in the array
            {
                roomParents[i].room = GameObject.FindGameObjectsWithTag("SpawnRooms")[i]; //Sets the room parent
                roomParents[i].spawnPoints = roomParents[i].room.GetComponentsInChildren<Transform>(); //Gathers and stores a list of all the room's spawnpoints into an array
                for (int x = 0; x < roomParents[i].spawnPoints.Length; x++)
                {
                    Debug.Log(roomParents[i].spawnPoints[x]);
                }
            }

            CmdUnlockRoom(0); //Calls upon UnlcokRoom to add the spawn room data to activeSpawners
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            Destroy(GameObject.FindWithTag("Enemy"));
        }

        if (isServer)
        {
            if (spawningPaused == true && GameManager.enemiesAlive < maxEnemies) //Checks whether or not the spawning can be unpaused based on the enemies alive(Uses spawningPaused as a gateway variable to stop an infinite loop)
            {
                spawningPaused = false;
            }

            if (enemiesSpawning == true && spawningPaused == false) //Checks whether or not enemies are needed to be spawned
            {
                int sT = 0; //Timer for enemy spawn rate
                int spawnCap = 0; //how many enemies can spawn at a time before the spawn timer increases
                for (int i = pausedIndex; i < activeSpawners.Count; i++) //Used to cycle through spawnpoints
                {
                    StartCoroutine(Spawn(i, sT, spawnCap)); //Spawns an enemy
                    GameManager.enemiesAlive++; //Adds enemy to the enemiesAlive counter
                    spawnCap++; //Raises the spawn cap
                    enemiesToSpawn--; //Decreases the enemiesToSpawn counter

                    if (spawnCap == activeSpawners.Count) //Checks whether or not all spawners have spawned one enemy before raising the spawn rate timer
                    {
                        sT += 3; //Adds to the existing spawn rate timer
                        spawnCap = 0; //Resets the spawn cap
                    }
                    if (enemiesToSpawn > 0 && i == activeSpawners.Count - 1) //Checks whether or not enemies are still needed to be spawned and if the spawnpoint's array has reached the final spawnpoint to allow for the spawning to loop
                    {
                        i = -1; //resets the loop
                    }
                    if (GameManager.enemiesAlive >= maxEnemies) //Checks whether or not the enemiesAlive has reached the max enemy spawn cap
                    {
                        spawningPaused = true; //Pauses the enemy spawning
                        pausedIndex = i + 1; //Stores i + 1 as the pausedIndex to allow for the spawning to continue from a new spawnpoint each time
                        if (pausedIndex == activeSpawners.Count - 1 || pausedIndex < 0) //Checks whether or not i has reached the activeSpawners count to make sure the spawners don't get stuck at only spawning at 0. Checks -1 as well.
                        {
                            pausedIndex = 0; //Sets to 0
                        }
                        i = activeSpawners.Count; //ends the loop
                    }
                    if (enemiesToSpawn == 0) //If the enemiesToSpawn count becomes 0, ends enemy spawning
                    {
                        pausedIndex = 0; //Resets pausedIndex once spawning has completed
                        i = activeSpawners.Count; //ends the loop
                        enemiesSpawning = false; //Ceases the ability to spawn more enemies
                        finishedSpawning = true; //Used to let the GameManager know the spawning has bee completed
                    }
                }
            }
        }
    }
    #endregion

    #region Add Spawners
    [Command]
    public void CmdUnlockRoom(int index) //Used to unlock a room to add the spawners to the activeSpawner List
    {
        for (int i = 1; i < roomParents[index].spawnPoints.Length; i++) //For all spawners within the room
        {
            activeSpawners.Add(roomParents[index].spawnPoints[i]); //Adds each spawner to the activeSpawner List
        }
    }
    #endregion

    IEnumerator Spawn(int spawnIndex, int spawnTime, int spawnCap)
    {
        yield return new WaitForSeconds(spawnTime); //Determines the amount of time between each set spawn
        GameObject enemy = Instantiate(enemyParent, activeSpawners[spawnIndex].position, Quaternion.identity, activeSpawners[spawnIndex]); //Instantiates a new enemy based on the spawnpoint arry's index
        NetworkServer.Spawn(enemy);
    }

    #region Spawn Enemies
    //void Spawn(int spawnIndex, int spawnTime, int spawnCap) //Used to spawn enemies
    //{
    //    StartCoroutine(Spawn(spawnIndex, spawnTime, spawnCap));
    //}
    #endregion
}
