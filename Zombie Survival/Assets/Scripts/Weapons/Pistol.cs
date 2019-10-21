using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Shoot()
    {
        if (currentClip > 0)
        {
            base.Shoot();
            currentClip--;
        }

    }

    public override void Reload()
    {
        base.Reload();
    }
}
