using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 *------------------------------------------------------------------------*/

public static class WeaponType
{
    public static Weapon AddWeapon(string Name)
    {
        int id = 0;
        float reloadTime = 0;
        float fireRate = 0;
        float spread = 0;
        float range = 25f;
        int clipSize = 8;
        int clip = clipSize;
        int ammo = 16;
        FireType function = FireType.Hitscan;
        GameObject gunObject = Resources.Load("Prefabs/Gun") as GameObject;

        switch (Name)
        {
            case "Pistol":
                id = 0;
                reloadTime = 2f;
                fireRate = 0.4f;
                spread = 1;
                function = FireType.Hitscan;
                break;
            case "Rifle":
                id = 1;
                reloadTime = 5f;
                fireRate = 0.3f;
                spread = 3;
                function = FireType.Hitscan;
                break;
            case "SMG":
                id = 2;
                reloadTime = 3.5f;
                fireRate = 0.1f;
                spread = 5;
                function = FireType.Projectile;
                break;
            case "Shotgun":
                id = 3;
                reloadTime = 2f;
                fireRate = 1f;
                spread = 1;
                function = FireType.Hitscan;
                break;
            case "Flamethrower":
                id = 4;
                reloadTime = 6f;
                fireRate = 0.2f;
                spread = 1;
                function = FireType.Entity;
                break;
            case "BFG":
                id = 5;
                reloadTime = 4f;
                fireRate = 3f;
                spread = 1;
                function = FireType.Projectile;
                break;
        }

        Weapon temp = new Weapon
        {
            Name = Name,
            ID = id,
            ReloadTime = reloadTime,
            FireRate = fireRate,
            Spread = spread,
            Range = range,
            ClipSize = clipSize,
            Clip = clip,
            Ammo = ammo,
            AmmoMax = ammo,
            Function = function,
            Gun = gunObject
        };
        return temp;
    }
}
