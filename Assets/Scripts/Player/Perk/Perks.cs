using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perks : MonoBehaviour
{
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

    public void ApplyStats(Player player) // This function assigns the player additioanl stats depending on the perk they have purchased
    {
        //player.instantRevive += instantRevive;
        //player.healthIncrease += healthIncrease;
        //player.damageIncrease += damageincrease;
        //player.fireRateIncrease += fireRateIncrease;
        //player.speedIncrease += speedIncrease;
        //player.lifeSteal += lifeSteal;
        //player.gunCapacityIncrease += gunCapacityIncrease;
    }

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
 public enum PerkType
    {
        HealthIncrease,
        DamageIncrease,
        SpeedIncrease,
        FireRateIncrease,
        InstantRevive,
        IncreasedGunCapacity,
        Lifesteal

    }