using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //Ammo:
    public int curAmmo;
    public int maxAmmo;
    public bool isFiring;
    public Text ammoDisplay;
    public Sprite perkIcon;
    public GameObject scoreboard;
    public Transform scoreHeader;
    public GameObject playerScore;

    public void Awake()
    {
        playerScore = Resources.Load("Prefabs/Player_Score") as GameObject;
        scoreboard = GameObject.Find("Scoreboard");
        scoreHeader = GameObject.Find("ScoreHeader").GetComponent<Transform>();
        scoreboard.SetActive(false);
    }

    public void Update()
    {
        //DisplayAmmo();

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

    //public void DisplayAmmo()
    //{
    //    ammoDisplay.text = curAmmo + "/" + maxAmmo;
    //    // ammoDisplay.text = maxAmmo.ToString();
    //    if (Input.GetMouseButtonDown(0) && !isFiring && curAmmo > 0)
    //    {
    //        isFiring = true;
    //        curAmmo--;
    //        isFiring = false;
    //    }
    //}

   // public void DisplayPerk()
   // {
   //     perkIcon.sprite = perkIcon;
   //     if()
   //     {

   //     }
   // }
}
