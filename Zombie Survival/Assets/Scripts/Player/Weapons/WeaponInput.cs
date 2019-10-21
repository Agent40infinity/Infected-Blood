using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInput : MonoBehaviour
{
    public KeyCode reloadKey, shootKey, aimKey;
    public WeaponBase myGun;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(shootKey))
        {
            myGun.Shoot();
        }
        if (Input.GetKeyDown(reloadKey))
        {
            myGun.Reload();
            Debug.Log("We have reloaded");
        }
    }
}
