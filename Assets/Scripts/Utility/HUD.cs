using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //Ammo:
    public Text curAmmo;
    public Text maxAmmo;
    public Text gunName;
    public bool isFiring;
    public Player localPlayer;
    public int weaponIndex;

    //Perks:
    public GameObject perks;
    public GameObject perkParent;
    public int perksAdded;

    //Scoreboard:
    public GameObject scoreboard;
    public Transform scoreHeader;
    public GameObject playerScore;

    public void Awake()
    {
        playerScore = Resources.Load("Prefabs/Player_Score") as GameObject;
        scoreboard = GameObject.Find("Scoreboard");
        perks = GameObject.Find("Perks");
        scoreHeader = GameObject.Find("ScoreHeader").GetComponent<Transform>();
        scoreboard.SetActive(false);
    }

    public void Update()
    {
        if (localPlayer != null)
        {
            DisplayAmmo();
            DisplayPerk();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
            displayStats();
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }

    public void displayStats()
    {
        for (int i = 0; i < GameManager.players.Length; i++)
        {
            GameObject scoreRef = Instantiate(playerScore, new Vector3(scoreHeader.position.x ,scoreHeader.position.y + (-i - 1 * 20), scoreHeader.position.z), Quaternion.identity, scoreHeader);
            Text[] textRef = scoreRef.GetComponentsInChildren<Text>();
            Debug.Log(textRef.Length);
            textRef[0].text = GameManager.playerName[i];
            textRef[1].text = GameManager.playerKills[i].ToString();
            textRef[2].text = GameManager.playerScore[i].ToString();
            textRef[3].text = GameManager.playerDowns[i].ToString();
            textRef[4].text = GameManager.playerDeaths[i].ToString();
        }
    }

    public void DisplayAmmo()
    {

        curAmmo.text = localPlayer.curWeapons[weaponIndex].Clip.ToString();
        maxAmmo.text = localPlayer.curWeapons[weaponIndex].Ammo.ToString();
        gunName.text = localPlayer.curWeapons[weaponIndex].Name;
    }

    public void DisplayPerk()
    {
        if (localPlayer.curPerks.Count > 0 && perksAdded < localPlayer.curPerks.Count )
        {
            for (int i = perksAdded; i < localPlayer.curPerks.Count; i++)
            {
                perksAdded++;
                Sprite perkIcon = localPlayer.curPerks[i].PerkIcon;
                GameObject perkRef = Instantiate(perkParent, new Vector3(20 + (i * 20), 5, perks.transform.position.z), Quaternion.identity, perks.transform);
                perkRef.GetComponent<SpriteRenderer>().sprite = perkIcon;
            }
        }
    }
}
