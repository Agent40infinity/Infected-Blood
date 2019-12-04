using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 *------------------------------------------------------------------------*/

public class Door : MonoBehaviour
{
    public GameObject[] doors;
    public int[] cost;
    public bool[] doorOpen;
    public EnemySpawner spawner;

    public void Awake()
    {
        doors = GameObject.FindGameObjectsWithTag("Door");
        spawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        cost = new int[] { 400, 600, 800, 800, 800, 1000, 1000, 1200, 2500, 2500 };
        doorOpen = new bool[] { false, false, false, false, false, false, false, false, false, false };
    }

    public void OpenDoor(int index)
    {
        Debug.Log("index: " + index);
        //doors[index].GetComponent<Animator>().SetBool("Open", true);
        doorOpen[index] = true;
        spawner.CmdUnlockRoom(index);
        doors[index].SetActive(false);
    }
}
