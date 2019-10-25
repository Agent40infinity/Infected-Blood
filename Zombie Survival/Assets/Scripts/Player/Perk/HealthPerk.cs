using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPerk : MonoBehaviour
{
    public bool showPerkMachineGUI = false; // The buy menu for the perk machine
    public int healthPerkPrice; // Price of the perk
    public AudioClip vendingSound; // Sound of the perk being dispenced

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        if (showPerkMachineGUI) // If the player is within the range of the perk machine this menu will populate allowing the player to purchase it
        {
            GUI.Label(new Rect(Screen.width - (Screen.width / 1.7f), Screen.height - // A small label for the perk machine
            (Screen.height / 1.4f), 800, 100), "Press key << E >> to buy perk $" + healthPerkPrice); // The text and price for the label
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
           // if (!GameManager.playerHasHealthPerk) // Error due to this not being added to game manager yet
                showPerkMachineGUI = true;
            //  else
            //   showPerkMachineGUI = false; Uncomment when this has been added to game manager
            //if (Input.GetKeyDown ("e") && GameManager.playerCash >= healthPerkPrice && showPerkMachineGUI)
            //    // If the player presses the E key within the buy zone and dont already have the perk and the player has enough currency then the player buys the perk
            //{
            //    GameManager.playerCash -= healthPerkPrice; // Cash is deducted and the player gets the perk
            //    GameManager.playerHasHealthPerk = true; // Telling the game manager to make it true so they player cant buy it twice
            // GetComponent<AudioSource>().PlayOneShot(vendingSound, 1.0f / GetComponent<AudioSource>().volume; This line plays the sound of the vending machine when a perk is purchased
            // other.SendMessage("GivePlayerPerk", "healthPerk"); Dispences the health perk to the player
            //}
        }
    }

    // In the game manager script add a public static bool (player1 has healthPerk = false;)
    // So the buy menu will disappear after the perk has been purchased
    /*
     * Add this to the player script
     * public float startingHitPoints; 
     * public void GivePlayerPerk(string whichPerk)
     * if(whichPerk == "healthPerk") (this if statement check if the perk is the health perk and applies the extra stats)
     * {
     *      maxHitPoints = maxHitPoints * 3;
     *      hitPoints = maxHitPoints;
     * }
     */
      
}
