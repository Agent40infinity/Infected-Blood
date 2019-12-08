using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 *------------------------------------------------------------------------*/

public class PerkMachine : MonoBehaviour
{
    #region Variables
    private int cost; //Stores the cost of the perk
    private PerkType perk; //Creates a reference to PerkType
    public string[] perkList; //Array to store the list of perks.
    #endregion

    #region Public Properties
    public int Cost //Creates a reference for Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    public PerkType Perk //Creates a refernce for Perk
    {
        get { return perk; }
        set { perk = value; }
    }
    #endregion

    #region General
    public void Awake()
    {
        perkList = System.Enum.GetNames(typeof(PerkType)); //Creates an array to store the name of each PerkType
        for (int i = 0; i < perkList.Length; i++) //Checks all perks.
        {
            if (gameObject.name.Contains(perkList[i])) //Checks whether or not the attached gameObjects contains the name of the perk and allows the perk to be purchased.
            {
                Perk = (PerkType)System.Enum.Parse(typeof(PerkType), perkList[i]);
                Debug.Log(perk);
            }
        }
    }
    #endregion
}
