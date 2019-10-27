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
    void Start()
    {
        doors = gameObject.GetComponentsInChildren<Animator>();
        cost = new int[] { 400, 600, 800, 800, 1000, 1000, 1000, 1200, 2500, 2500 };
    }

    public void OpenDoor(int index)
    {
        doors[index].SetBool("Open", true);
        doorOpen[index] = true;
    }
}
