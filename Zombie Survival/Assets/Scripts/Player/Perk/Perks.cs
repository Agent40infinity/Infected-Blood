using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perks : MonoBehaviour
{
    private int id;
    private new string name;
    private float damageIncrease;
    private float healthIncrease;
    private float gunCapacityincrease;
    private float lifeSteal;
    private float instantRevive;
    private float speedIncrease;
    private float fireRateIncrease;
    private Texture2D perkIcon;
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
    public float GunCapacityincrease
    {
        get { return gunCapacityincrease; }
        set { gunCapacityincrease = value; }
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
    public Texture2D PerkIcon
    {
        get { return perkIcon; }
        set { perkIcon = value; }
    }
   
    public PerkType Perk
    {
        get { return perk; }
        set { perk = value; }
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