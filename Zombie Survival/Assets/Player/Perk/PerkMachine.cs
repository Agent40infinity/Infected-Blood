using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMachine : MonoBehaviour
{
    private int cost;
    private PerkType perk;
    public string[] perkList;

    public int Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    public PerkType Perk
    {
        get { return perk; }
        set { perk = value; }
    }

    public void Awake()
    {
        perkList = System.Enum.GetNames(typeof(PerkType));
        for (int i = 0; i < perkList.Length; i++)
        {
            if (gameObject.name.Contains(perkList[i]))
            {
                Perk = (PerkType)System.Enum.Parse(typeof(PerkType), perkList[i]);
                Debug.Log(perk);
            }
        }
    }
}
