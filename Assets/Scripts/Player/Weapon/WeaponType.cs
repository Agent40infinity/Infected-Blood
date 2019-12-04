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
        int damage = 25;
        float reloadTime = 0;
        float fireRate = 0;
        float spread = 0;
        float range = 25f;
        int clipSize = 8;
        int clip = 8;
        int ammo = 16;
        int AmmoMax = 0;
        string icon = "";
        FireType function = FireType.Hitscan;
        string gunObject = "Gun";


        switch (Name)
        {
            case "Pistol":
                id = 0;
                reloadTime = 2f;
                clipSize = 8;
                clip = 8;
                AmmoMax = 64;
                fireRate = 0.4f;
                spread = 1;
                function = FireType.Hitscan;
                icon = "Pistol";
                break;
            case "Rifle":
                id = 1;
                reloadTime = 5f;
                clipSize = 30;
                clip = 30;
                AmmoMax = 240;
                fireRate = 0.3f;
                spread = 3;
                function = FireType.Hitscan;
                break;
            case "SMG":
                id = 2;
                reloadTime = 3.5f;
                clipSize = 45;
                clip = 45;
                AmmoMax = 360;
                fireRate = 0.1f;
                spread = 5;
                function = FireType.Projectile;
                break;
            case "Shotgun":
                id = 3;
                reloadTime = 2f;
                clipSize = 8;
                clip = 8;
                AmmoMax = 64;
                fireRate = 1f;
                spread = 1;
                function = FireType.Hitscan;
                break;
            case "Flamethrower":
                id = 4;
                reloadTime = 6f;
                clipSize = 100;
                clip = 100;
                AmmoMax = 800;
                fireRate = 0.2f;
                spread = 1;
                function = FireType.Entity;
                break;
            case "BFG":
                id = 5;
                reloadTime = 4f;
                clipSize = 4;
                clip = 4;
                AmmoMax = 20;
                fireRate = 3f;
                spread = 1;
                function = FireType.Projectile;
                break;
        }

        Weapon temp = new Weapon
        {
            Name = Name,
            ID = id,
            Damage = damage,
            ReloadTime = reloadTime,
            FireRate = fireRate,
            Spread = spread,
            Range = range,
            ClipSize = clipSize,
            Clip = clip,
            Ammo = ammo,
            Icon = Resources.Load("Icon/"+icon) as Sprite,
            AmmoMax = ammo,
            Function = function,
            Gun = Resources.Load("Prefabs/"+ gunObject) as GameObject
        };
        return temp;

    }
}
