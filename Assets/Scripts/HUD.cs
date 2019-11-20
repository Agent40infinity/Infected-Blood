using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public int curAmmo;
    public int maxAmmo;
    public bool isFiring;
    public Text ammoDisplay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ammoDisplay.text = curAmmo +"/"+ maxAmmo;
       // ammoDisplay.text = maxAmmo.ToString();
        if(Input.GetMouseButtonDown(0) && !isFiring && curAmmo >0)
        {
            isFiring = true;
            curAmmo--;
            isFiring = false;
        }
    }
}
