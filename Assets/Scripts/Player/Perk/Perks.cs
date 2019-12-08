using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan w/ Jack.
 *------------------------------------------------------------------------*/

public class Perks : MonoBehaviour
{
    #region Private Variables
    private int id;
    private new string name;
    private float damageIncrease;
    private float healthIncrease;
    private float gunCapacityIncrease;
    private float lifeSteal;
    private float instantRevive;
    private float speedIncrease;
    private float fireRateIncrease;
    private Sprite perkIcon;
    private PerkType perk;
    #endregion

    #region Public Properties
    public int ID
    {
        get { return id; }
        set { id = value; }
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public float DamageIncrease
    {
        get { return damageIncrease; }
        set { damageIncrease = value; }
    }
    public float HealthIncrease
    {
        get { return healthIncrease; }
        set { healthIncrease = value; }
    }
    public float GunCapacityIncrease
    {
        get { return gunCapacityIncrease; }
        set { gunCapacityIncrease = value; }
    }
    public float LifeSteal
    {
        get { return lifeSteal; }
        set { lifeSteal = value; }
    }
    public float InstantRevive
    {
        get { return instantRevive; }
        set { instantRevive = value; }
    }
    public float SpeedIncrease
    {
        get { return speedIncrease; }
        set { speedIncrease = value; }
    }
    public float FireRateIncrease
    {
        get { return fireRateIncrease; }
        set { fireRateIncrease = value; }
    }
    public Sprite PerkIcon
    {
        get { return perkIcon; }
        set { perkIcon = value; }
    }
   
    public PerkType Perk
    {
        get { return perk; }
        set { perk = value; }
    }
    #endregion

    #region Apply Stats
    public void ApplyStats(Player player) // This function assigns the player additioanl stats depending on the perk they have purchased
    {
        //Weapon Based:
        for (int i = 0; i < player.curWeapons.Count; i++)
        {
            if (!player.curWeapons[i].BeenModified)
            {
                //DamageIncrease;
                float refDamage = player.curWeapons[i].Damage;
                refDamage *= DamageIncrease;
                player.curWeapons[i].Damage = (int)refDamage;

                //FireRateIncrease:
                player.curWeapons[i].FireRate *= FireRateIncrease;

                //CapacityIncrease:
                float refCapacity = player.curWeapons[i].AmmoMax;
                refCapacity *= GunCapacityIncrease;
                player.curWeapons[i].AmmoMax = (int)refCapacity;

                player.curWeapons[i].BeenModified = true;
                Debug.Log(player.curWeapons[i].BeenModified);
            }
        }

        //HealthIncrease:
        if (!player.statModified[0] && Perk == PerkType.HealthIncrease) //Speed modified check
        {
            float refHealth = player.maxHealth;
            refHealth *= healthIncrease;
            player.maxHealth = (int)refHealth;
            player.statModified[0] = true;
        }

        //SpeedIncrease:
        if (!player.statModified[1] && Perk == PerkType.SpeedIncrease) //Speed modified check
        {
            player.speed *= SpeedIncrease;
            player.statModified[1] = true;
        }

        //InstantRevive:
        if (!player.statModified[2] && Perk == PerkType.InstantRevive) //Instant Revive modified check
        {
            player.pickupTime = instantRevive;
            player.statModified[2] = true;
        }

        //player.lifeSteal += lifeSteal;        
    }
    #endregion

    public void OnDestroy() // If the player were to go down and or die the perks they currently hold are removed
    {
        //player.maxhealth -= healthIncrease;
        //player.instantRevive -= instantRevive;
        //player.damageIncrease -= damageIncrease;
        //player.fireRateincrease -= fireRateIncrease;
        //player.speedIncrease -= speedIncrease;
        //player.lifeSteal -= lifeSteal;
        //player.gunCapacityIncrease -= gunCapacityIncrease;
    }
}
 public enum PerkType //List of Enums created to categorize each perk.
{
        HealthIncrease,
        DamageIncrease,
        SpeedIncrease,
        FireRateIncrease,
        InstantRevive,
        IncreasedGunCapacity,
        Lifesteal

    }