using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject scoreboard;

    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreboard = GameObject.Find("Scoreboard");
    }

    public void Update()
    {
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

    public void BeginGame()
    {
        gameManager.StartGame();
    }
}
