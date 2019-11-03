using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVendor : MonoBehaviour
{
    private int cost;
    private string weaponName;

    public int Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    public string WeaponName
    {
        get { return weaponName; }
        set { weaponName = value; }
    }

    public void Awake()
    {
        string name = gameObject.name.Replace("_GunVendor", "");
        WeaponName = name;

        Debug.Log(weaponName);
    }
}
