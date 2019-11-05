using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 *------------------------------------------------------------------------*/

public class GameManager : MonoBehaviour
{
    //General
    public bool enemiesDead = true;
    public int round = 0;
    public int enemiesPerRound = 8;
    public static int enemiesAlive;
    public static List<Player> playersDead = new List<Player>();
    public Transform[] spawnPos;

    GameObject[] players;
    public int[] playerKills = new int[4];

    //References:
    public EnemySpawner enemySpawner;

    public void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        spawnPos = GameObject.Find("SpawnPos").GetComponentsInChildren<Transform>();
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    public void Update()
    {
        Rounds();
        GetPlayerData();
    }

    public void GetPlayerData()
    {
        for (int i = 0; i < players.Length; i++)
        {
            Player playerRef = players[i].GetComponent<Player>();
            playerKills[i] = playerRef.kills; 
        }
    }

    public void Rounds()
    {
        if (enemiesDead == true && enemySpawner.finishedSpawning == true) //Checks whether or not all enemies are dead and that all enemies have ceased spawning to allow for a new round to start
        {
            round++;
            if (round > 1)
            {
                StartCoroutine(IncreaseDifficulty());
            }
            StartCoroutine(RevivePlayers());
            enemySpawner.enemiesToSpawn = enemiesPerRound;
            enemySpawner.enemiesSpawning = true;
            enemySpawner.finishedSpawning = false;
            enemiesDead = false;
        }

        if (enemiesAlive == 0 && enemySpawner.finishedSpawning == true && enemiesDead == false)
        {
            enemiesDead = true;
        }
    }

    IEnumerator RevivePlayers()
    {
        for (int i = 0; i < playersDead.Count; i++)
        {
            StartCoroutine(playersDead[i].Revived(spawnPos[i + 1]));
            playersDead.RemoveAt(i);
        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator IncreaseDifficulty()
    {
        if (round < 10)
        {
            enemiesPerRound = (int)(enemiesPerRound * 1.4f);
        }
        else if (enemiesPerRound < 250)
        {
            enemiesPerRound = (int)(enemiesPerRound * 1.15f);
        }
        yield return new WaitForEndOfFrame();
    }
}
