using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 *------------------------------------------------------------------------*/

public class HUD : MonoBehaviour
{
    #region Variables
    //Screen:
    public Vector2 scrt; //Used to store the values for the screen size

    //Ammo:
    public Text curAmmo; //Reference to the curAmmo text
    public Text maxAmmo; //Reference to the maxAmmo text
    public Text gunName; //Reference to the gunName text
    public Player localPlayer; //Refence to the local player
    public int weaponIndex; //Reference for the selected weapon as an index

    //Perks:
    public GameObject perks; //Reference to the perks
    public GameObject perkParent; //Reference to the perk parent gameObject for display
    public int perksAdded; //Reference to check how many perks have been added

    //Scoreboard:
    public GameObject scoreboard; //Reference to the scoreboard
    public Transform scoreHeader; //Reference to the scoreHeader
    public GameObject playerScore; //Reference to the player's score
    #endregion

    #region General:
    public void Awake() //Gets the screen size and gathers all of the relevant references needed
    {
        scrt.x = Screen.width / 16;
        scrt.y = Screen.height / 9;

        playerScore = Resources.Load("Prefabs/Player_Score") as GameObject;
        scoreboard = GameObject.Find("Scoreboard");
        perks = GameObject.Find("Perks");
        scoreHeader = GameObject.Find("ScoreHeader").GetComponent<Transform>();
        scoreboard.SetActive(false);
    }

    public void Update()
    {
        DisplayMoney(); 

        if (localPlayer != null) //Displays the Ammo and Perks only to the local player
        {
            DisplayAmmo();
            DisplayPerk();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) //Checks whether or not you're pressing tab to check stats
        {
            scoreboard.SetActive(true);
            displayStats();
        }
        else if (Input.GetKeyUp(KeyCode.Tab)) //If you let go of tab, turns off stats
        {
            scoreboard.SetActive(false);
        }
    }
    #endregion

    #region Display
    public void displayStats() //Used to display the stats
    {
        for (int i = 0; i < GameManager.players.Length; i++) //For each player, displays the relevant stats tied to the player in a list 
        {
            GameObject scoreRef = Instantiate(playerScore, new Vector3(scoreHeader.position.x ,scoreHeader.position.y + (-i - 1 * (scrt.y * 0.3f)), scoreHeader.position.z), Quaternion.identity, scoreHeader);
            Text[] textRef = scoreRef.GetComponentsInChildren<Text>();
            textRef[0].text = GameManager.playerName[i];
            textRef[1].text = GameManager.playerScore[i].ToString();
            textRef[2].text = GameManager.playerKills[i].ToString();
            textRef[3].text = GameManager.playerDowns[i].ToString();
            textRef[4].text = GameManager.playerDeaths[i].ToString();
        }
    }

    public void DisplayAmmo() //Used to display the ammo
    {

        curAmmo.text = localPlayer.curWeapons[weaponIndex].Clip.ToString();
        maxAmmo.text = localPlayer.curWeapons[weaponIndex].Ammo.ToString();
        gunName.text = localPlayer.curWeapons[weaponIndex].Name;
    }

    public void DisplayPerk() //Used to display the players documentation
    {
        if (localPlayer.curPerks.Count > 0 && perksAdded < localPlayer.curPerks.Count ) //Checks whether or not the curPerk count is above 0 and that the local player has less than the current counted perks
        {
            for (int i = perksAdded; i < localPlayer.curPerks.Count; i++) //Displays the perk and instantiates the display gameObject.
            {
                perksAdded++;
                Sprite perkIcon = localPlayer.curPerks[i].PerkIcon;
                GameObject perkRef = Instantiate(perkParent, new Vector3((scrt.x * 0.5f) + i * (scrt.x * 0.4f), scrt.x * 0.5f, perks.transform.position.z), Quaternion.identity, perks.transform);
                perkRef.GetComponent<SpriteRenderer>().sprite = perkIcon;
            }
        }
    }

    public void DisplayMoney()
    {

    }
    #endregion
}
