using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 *------------------------------------------------------------------------*/

public class Door : MonoBehaviour
{
    #region Variables
    //General:
    public GameObject[] doors; //Array to hold all the door gameObjects.
    public int[] cost; //Array to determine the price of each door.
    public bool[] doorOpen; //Array to check whether or not the door has been openned already.

    //References:
    public EnemySpawner spawner; //Reference to the enemySpawner.
    #endregion

    #region General
    public void Awake() //Sets up all the values.
    {
        doors = GameObject.FindGameObjectsWithTag("Door");
        spawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        cost = new int[] { 400, 600, 800, 800, 800, 1000, 1000, 1200, 2500, 2500 };
        doorOpen = new bool[] { false, false, false, false, false, false, false, false, false, false };
    }
    #endregion

    #region Open Door
    public void OpenDoor(int index) //Allows the door to be openned and adds the corrosponding spawners for that openned room.
    {
        Debug.Log("index: " + index);
        //doors[index].GetComponent<Animator>().SetBool("Open", true);
        doorOpen[index] = true;
        spawner.CmdUnlockRoom(index + 1);
        doors[index].SetActive(false);
    }
    #endregion
}
