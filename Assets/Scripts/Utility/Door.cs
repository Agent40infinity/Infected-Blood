using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 *------------------------------------------------------------------------*/

public class Door : MonoBehaviour
{
    public Animator[] doors;
    public int[] cost;
    public bool[] doorOpen;
    public EnemySpawner spawner;

    public void Awake()
    {
        doors = gameObject.GetComponentsInChildren<Animator>();
        spawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        cost = new int[] { 400, 600, 800, 800, 1000, 1000, 1000, 1200, 2500, 2500 };
        doorOpen = new bool[] { false, false, false, false, false, false, false, false, false, false };
    }

    public void OpenDoor(int index)
    {
        doors[index].SetBool("Open", true);
        doorOpen[index] = true;
        spawner.CmdUnlockRoom(index + 1);
    }
}
