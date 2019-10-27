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

    //References:
    public EnemySpawner enemySpawner;

    public void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }

    public void Update()
    {
        Rounds();
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
