using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerkData
{
    public static Perks AddPerk(PerkType Type)
    {
        int id = 0;
        string name = "";
        string perkIcon = "";
        float damageIncrease = 0f;
        float healthIncrease = 0f;
        float gunCapacityincrease = 0f;
        float lifeSteal = 0f;
        float instantRevive = 0f;
        float speedIncrease = 0f;
        float fireRateIncrease = 0f;

        // PerkType;
        switch (Type)
        {
            case PerkType.HealthIncrease:
                id = 0;
                name = "frank";
                perkIcon = "Health";
                healthIncrease = 0.50f;
                break;
            case PerkType.DamageIncrease:
                id = 1;
                name = "";
                perkIcon = "Damage";
                damageIncrease = 0.20f;
                break;
            case PerkType.SpeedIncrease:
                id = 2;
                name = "";
                perkIcon = "Speed";
                speedIncrease = 0.25f;
                break;
            case PerkType.FireRateIncrease:
                id = 3;
                name = "";
                perkIcon = "Firerate";
                fireRateIncrease = 0.25f;
                break;
            case PerkType.InstantRevive:
                id = 4;
                name = "";
                perkIcon = "InstantRevive";
                instantRevive = 1f;
                break;
            case PerkType.IncreasedGunCapacity:
                id = 5;
                name = "";
                perkIcon = "Capacity";
                gunCapacityincrease = 0.50f;
                break;
            case PerkType.Lifesteal:
                id = 6;
                name = "";
                perkIcon = "Lifesteal";
                lifeSteal = 0.10f;
                break;

        }
        Perks temp = new Perks()
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
