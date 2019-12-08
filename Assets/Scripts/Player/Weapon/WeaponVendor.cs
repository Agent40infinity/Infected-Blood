using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 *------------------------------------------------------------------------*/

public class WeaponVendor : MonoBehaviour
{
    #region private variables
    //General:
    private int cost; //Stores the cost of the Weapon being bought
    private string weaponName; //Stores the weapon's name
    #endregion

    #region public variables
    public int Cost //Public property to allow for the calling of Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    public string WeaponName //Public property to allow for the calling of WeaponName
    {
        get { return weaponName; }
        set { weaponName = value; }
    }
    #endregion

    #region General
    public void Awake() //Used to check and determine what weapon is being sold at the attached Vendor
    {
        string name = gameObject.name.Replace("_GunVendor", "");
        WeaponName = name;

        Debug.Log(weaponName);
    }
    #endregion
}
