using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : MonoBehaviour
{
    //General:
    private string name;
    private int id;
    private float reloadTime;
    private float fireRate;
    private float spread;
    private float range;
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

    public float Range
    {
        get { return range; }
        set { range = value; }
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

    public void Shoot(Camera playerCam, GameObject player)
    {
        switch (function)
        {
            case FireType.Hitscan:
                Vector3 mousePosition = playerCam.ScreenToWorldPoint(Vector3.zero);
                RaycastHit hit;
                Debug.Log("Referenced");
                if (Physics.Raycast(player.transform.position, mousePosition, out hit, Range))
                {
                    Debug.Log("Out");

                    if (hit.collider.tag == "Enemy")
                    {
                        Debug.Log("En");

                        hit.collider.GetComponent<Enemy>().Death();
                    }
                }
                break;
            case FireType.Projectile:

                break;
            case FireType.Entity:

                break;
            case FireType.Shotgun:

                break;
        }
    }
}

public enum FireType
{
    Hitscan,
    Projectile,
    Entity,
    Shotgun
}
