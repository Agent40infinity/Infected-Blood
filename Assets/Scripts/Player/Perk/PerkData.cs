using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------
 * Script Created by: Jack w/ Aiden Nathan.
 *------------------------------------------------------------------------*/

public static class PerkData
{
    public static Perks AddPerk(PerkType Type) //Used to add a perk to a List
    {
        //Variable references to allow for a private setting.
        int id = 0;
        string name = "";
        string perkIcon = "";
        float damageIncrease = 1f;
        float healthIncrease = 1f;
        float gunCapacityincrease = 1f;
        float lifeSteal = 0f;
        float instantRevive = 1f;
        float speedIncrease = 1f;
        float fireRateIncrease = 1f;

        // PerkType;
        switch (Type) //Used to determine what type of perk is being added and changes the stats modifed in accordance.
        {
            case PerkType.HealthIncrease:
                id = 0;
                name = "frank";
                perkIcon = "Health";
                healthIncrease = 1.50f;
                break;
            case PerkType.DamageIncrease:
                id = 1;
                name = "";
                perkIcon = "Damage";
                damageIncrease = 1.20f;
                break;
            case PerkType.SpeedIncrease:
                id = 2;
                name = "";
                perkIcon = "Speed";
                speedIncrease = 1.25f;
                break;
            case PerkType.FireRateIncrease:
                id = 3;
                name = "";
                perkIcon = "Firerate";
                fireRateIncrease = 0.75f;
                break;
            case PerkType.InstantRevive:
                id = 4;
                name = "";
                perkIcon = "InstantRevive";
                instantRevive = 0f;
                break;
            case PerkType.IncreasedGunCapacity:
                id = 5;
                name = "";
                perkIcon = "Capacity";
                gunCapacityincrease = 1.50f;
                break;
            case PerkType.Lifesteal:
                id = 6;
                name = "";
                perkIcon = "Lifesteal";
                lifeSteal = 0.10f;
                break;

        }
        Perks temp = new Perks() //Creates the data for a new perk and applies the temp/private modifications to said perk then returns the perk as the value for AddPerk.
        {
            ID = id,
            Name = name,
            DamageIncrease = damageIncrease,
            SpeedIncrease = speedIncrease,
            FireRateIncrease = fireRateIncrease,
            InstantRevive = instantRevive,
            GunCapacityIncrease = gunCapacityincrease,
            LifeSteal = lifeSteal,
            HealthIncrease = healthIncrease,
            Perk = Type,
            PerkIcon = Resources.Load("Icon/"+ perkIcon) as Sprite
        };
        return temp;
    }

}
