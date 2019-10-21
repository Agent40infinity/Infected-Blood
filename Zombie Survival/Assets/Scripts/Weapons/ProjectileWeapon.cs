using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : WeaponBase
{
    public GameObject projectile;

    public float force = 100f;
    
    
    public override void Shoot()
    {
        if (currentClip > 0)
        {
            GameObject spawnedObject = Instantiate(projectile, muzzle.position, muzzle.rotation);

            spawnedObject.GetComponent<Rigidbody>().AddForce(muzzle.forward * force);

            currentClip--;
        }
        
    }

    //public override void Reload()
    //{
        
    //}
}
