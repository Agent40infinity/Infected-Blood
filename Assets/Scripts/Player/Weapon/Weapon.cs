using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 *------------------------------------------------------------------------*/

[System.Serializable]
public class Weapon : NetworkBehaviour
{
    #region Private Variables
    //General:
    private new string name;
    private int id;
    private int damage;
    private float reloadTime;
    private float fireRate;
    private float spread;
    private float range;
    private int ammo;
    private int ammoMax;
    private int clipSize;
    private int clip;
    private bool beenModified;
    private FireType function;
    private Sprite icon;

    //References:
    private GameObject gunObject;
    #endregion

    #region Public Properties
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
   public Sprite Icon
    {
        get { return icon; }
        set { icon = value; }
    }
    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
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

    public int ClipSize
    {
        get { return clipSize; }
        set { clipSize = value; }
    }

    public int Clip
    {
        get { return clip; }
        set { clip = value; }
    }

    public int Ammo
    {
        get { return ammo; }
        set { ammo = value; }
    }

    public int AmmoMax
    {
        get { return ammoMax; }
        set { ammoMax = value; }
    }

    public bool BeenModified
    {
        get { return beenModified; }
        set { beenModified = value; }
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
    #endregion

    #region Shooting
    [Client]
    public void Shoot(Camera playerCam, GameObject gun, Player player) 
    {
        switch (function) //Switches between the different functions of each weapon and fires in accordance to that type
        {
            case FireType.Hitscan: //All things to do with Hitscan
                Vector3 mousePosition = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                Debug.DrawRay(mousePosition, playerCam.transform.forward * range, Color.red, 5f);
                if (Physics.Raycast(mousePosition, playerCam.transform.forward, out hit, range)) //Shoots a raycast out and checks whether or not it hits anything.
                {
                    if (hit.collider.tag == "Enemy") //If the raycast hits an enemy, deal damage to said enemy
                    {
                        GameObject particlePrefab = Resources.Load<GameObject>("Prefabs/Particles/Hit-Marker-Particle");
                        GameObject bulletTracer = Instantiate(particlePrefab, hit.point, Quaternion.LookRotation(playerCam.transform.forward));
                        hit.collider.gameObject.GetComponent<Enemy>().CmdTakeDamage(damage, player);
                    }
                    else if(hit.collider.tag != "Environment") //If the raycast hits the environment, don't do anything
                    {
                        GameObject particlePrefab = Resources.Load<GameObject>("Prefabs/Particles/Impact-Particle");
                        GameObject bulletTracer = Instantiate(particlePrefab, hit.point, Quaternion.LookRotation(playerCam.transform.forward));
                    }
                }
                break;
            case FireType.Projectile: //All things to do with projectile
                GameObject bullet = Resources.Load<GameObject>("Prefabs/Bullet");
                Instantiate(bullet, transform.position, Quaternion.identity);
                Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
                bulletRigid.AddForce(Vector3.forward * 100f, ForceMode.Impulse);
                break;
            case FireType.Entity:

                break;
            case FireType.Shotgun:

                break;
        }
    }
    #endregion
}

public enum FireType
{
    Hitscan,
    Projectile,
    Entity,
    Shotgun
}
