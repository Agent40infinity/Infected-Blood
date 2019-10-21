using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //General:
    private string name;
    private int id;
    private float reloadTime;
    private float fireRate;
    private float spread;
    private FireType function;

    //References:
    private GameObject gunObject;

    //Public Properties:
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public float ReloadTime
    {
        get { return reloadTime; }
        set { reloadTime = value; }
    }

    public float FireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
    }

    public float Spread
    {
        get { return spread; }
        set { spread = value; }
    }

    public FireType Function
    {
        get { return function; }
        set { function = value; }
    }

    public GameObject Gun
    {
        get { return gunObject; }
        set { gunObject = value; }
    }
}

public enum FireType
{
    Hitscan,
    Projectile,
    Entity,
    Shotgun
}
