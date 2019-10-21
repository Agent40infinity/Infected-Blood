using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public int damage;
    public int maxAmmo;
    public int currentAmmo;
    public int maxClip;
    public int currentClip;
    public float weaponRange;
    public float reloadSpeed;
    public float fireRate;
    public Transform muzzle;

    public int tempClip;

    public void Start() // Start the game with full ammo and clip
    {
        currentAmmo = maxAmmo;
        currentClip = maxClip;

    }
    public virtual void Shoot()
    {
        Debug.Log("bang!");

    }

    public virtual void Reload()
    {
        // If max ammo is not empty
        if (currentAmmo > 0)
        {
            Debug.Log("we got bullets now");
            // If the current clip is empty or has some ammo remaining 
            if (currentClip >= 0)
            {
                // Refund clip into max ammo
                currentAmmo += currentClip;
                // Set the clip to empty
                currentClip = 0;
                // If there is enough to fill the clip
                if (currentAmmo >= maxClip)
                {
                    // Remove ammo from the reserve equal to the clip size
                    currentAmmo -= maxClip - currentClip;
                    // Fill the clip
                    currentClip = maxClip;
                }
                // Else, if there is not enough ammo to fill the clip
                else if (currentAmmo < maxClip)
                {
                    tempClip = currentAmmo;
                    currentClip = tempClip;
                    currentAmmo -= tempClip;
                }
            }
        }
    }
}
