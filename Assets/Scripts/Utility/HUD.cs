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
    public GameManager gameManager;
    public GameObject scoreboard;

    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreboard = GameObject.Find("Scoreboard");
    }

    public void Update()
    {
        DisplayAmmo();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
            displayStats();
        }
        else
        {
            scoreboard.SetActive(false);
        }
    }

    public void displayStats()
    {
        
    }

    public void DisplayAmmo()
    {
        ammoDisplay.text = curAmmo + "/" + maxAmmo;
        // ammoDisplay.text = maxAmmo.ToString();
        if (Input.GetMouseButtonDown(0) && !isFiring && curAmmo > 0)
        {
            isFiring = true;
            curAmmo--;
            isFiring = false;
        }
    }

   // public void DisplayPerk()
   // {
   //     perkIcon.sprite = perkIcon;
   //     if()
   //     {

   //     }
   // }

    public void BeginGame()
    {
        gameManager.StartGame();
    }
}
