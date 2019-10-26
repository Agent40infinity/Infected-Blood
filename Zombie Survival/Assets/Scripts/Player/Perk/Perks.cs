using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perks : MonoBehaviour
{
    private int id = 0;
    private new string name;
    private float damageIncrease = 0.20f;
    private float healthIncrease = 0.50f;
    private float gunCapacityincrease = 0.50f;
    private float lifeSteal = 0.10f;
    private float instantRevive = 1f;
    private float speedIncrease = 0.25f;
    private float fireRateIncrease = 0.25f;
    private Texture2D perkIcon;


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