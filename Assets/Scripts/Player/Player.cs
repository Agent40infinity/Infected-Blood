using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Mirror;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 * Frankensteined camera movement from Jaymie.
 -------------------------------------------------------------------------*/

[RequireComponent(typeof(CharacterController))]
public class Player : NetworkBehaviour
{
    #region Variables
    //Sound:
    [Header("Sound Effects/Music")]
    public AudioSource rifleSound, shotgunSound, walkSound;

    //Statistics:
    [Header("Statistics")]
    public int score = 0; //Stat to keep track of how much score the player has.
    public int money = 0; //Stat to keep track of how much money the player can spend.
    public int downs = 0; //Stat to keep track of how many times the player has been downed.
    public int kills = 0; //Stat to keep track of how many kills the player has.
    public int deaths = 0; //Stat to keep track of how many times the player died.

    //Movement:
    [Header("Physics")]
    public Vector3 moveDirection; //Vector3 used to store the movement values.
    public float jumpSpeed = 12; //speed applied to jumping.
    public float speed = 3; //speed applied for default movement.
    public float sprintMultiplier = 1.5f; //speed applied when sprinting.
    public float gravity = 20; //default return value for gravity.
    public float appliedGravity; //gravity used to affect the player.
    public float gravityIncreaseTimer; //Timer used to increase appliedGravity's effect over time.
    public float increaseTimer = 0; //Counter used to disable the players ability to increase the sprintSpeed after a certain time frame.

    public Vector3 groundedOverlay = new Vector3(0.2f, 0.1f, 0.2f); //Vector3 used to store the values for the collider used to check for isGrounded.
    public Vector3 groundDistance = new Vector3(0f, -1f, 0f); //Vector3 used to store the position of the isGrounded check.

    //MouseMovement: 
    [Header("Camera Controller")]
    [Range(0, 20)]
    public float sensX = 15; //Sensitivity for X axis of mouse.
    [Range(0, 20)]
    public float sensY = 15; //Sensitivity for Y axis of mouse.
    public float minY = -65; //Max value for the rotation on the Y axis for the camera.
    public float maxY = 65; //Min value for the rotation on the Y axis for the camera.
    public float rotationY = 0; //value to store the Y axis for the rotation.

    //Weapon Management:
    [Header("Weapon Management")]
    public List<Weapon> curWeapons = new List<Weapon>(); //List of the current weapons.
    public float shotCooldown; //Cooldown for shooting based on the selected weapons firrate.
    public bool canFire = true; //Checks whether or not the player can shoot.
    public int selectedIndex = 0; //Selected Index for determining what weapon the player has selected.

    //Perk Management:
    [Header("Perk Management")]
    public List<Perks> curPerks = new List<Perks>(); //List of all current perks.
    public bool[] statModified = { false, false, false }; //Health, Speed, InstantRevive;

    //Interactions:
    [Header("Interactions")]
    public float interactRange = 10f; //Range for how far the player can interact with the environment/entities.
    public bool revivingPlayer = false; //Checks whether or not the player is attempting to revive another player.
    public float pickupTime = 3f; //timer for the pickup time of the other player.
    public bool canInteract = false; //Checks whether or not the player is able to interact with the environment/entities.

    //Health Management:
    [Header("Health Management")]
    public int curHealth; //Current health of the player.
    public int maxHealth = 100; //Max health of the player.
    public bool isDowned = false; //Checks whether or not the player is downed and needs to be revived.
    public bool beingRevived = false; //Checks whether or not the player is being revived
    public bool playerDead = false; //Checks whether or not the player is dead.
    public float timeTillDeath = 0; //Timer to determine how long the player has left before they can no longer be revived.
    public float deathTime = 15f;  //Default value for the time left till death.
    public bool canTakeDamage = true; //Checks whether or not the player can be damaged.

    //References:
    [Header("References")]
    public CharacterController controller; //Reference for the attached character controller.
    public GameObject camera; //Reference for the attached camera.
    public GameObject hand; //Reference to allow for the instantiating of each weapon.
    public LayerMask ground; //Reference for the LayerMask that stores the value for ground.
    public HUD hud; //Reference to the HUD display.

    public bool IsGrounded() //Used to determine if the player is grounded based on a collider check.
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.localPosition + groundDistance, groundedOverlay, Quaternion.identity, ground); //Creates a overlap box to check whether the player is grounded.
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.layer == 9) //Checks each gameObject that the collider hits to see if it is layered as "ground".
            {
                return true;
            }
        }
        return false;
    }

    int GetNumberFromString(string word) //Allows for the trasnlation of strings into integers.
    {
        string number = Regex.Match(word, @"\d+").Value;

        int result;
        if (int.TryParse(number, out result))
        {
            return result;
        }
        return -1;
    }

    bool PerkCheck(PerkType perkInput) //Checks whether or not you already have the perk you are attempting to buy.
    {
        for (int i = 0; i < curPerks.Count; i++)
        {
            if (curPerks[i].Perk == perkInput)
            {
                return true;
            }
        }
        return false;
    }

    bool WeaponCheck(string weaponInput) //Checks whether or not you already have the weapon you are attempting to buy.
    {
        for (int i = 0; i < curWeapons.Count; i++)
        {
            if (curWeapons[i].Name == weaponInput)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region General
    public void Start() //Used to determine default values and grab references.
    {
        if (isLocalPlayer) //Checks whether or not the player script is attached to the local player.
        {
            isDowned = false;
            playerDead = false;
            curHealth = maxHealth;
            controller = gameObject.GetComponent<CharacterController>();
            camera.SetActive(true);
            //hand = GameObject.Find("Hand");
            curWeapons.Add(WeaponType.AddWeapon("Pistol"));
            Instantiate(curWeapons[0].Gun, hand.transform.position, Quaternion.identity, hand.transform);
            shotCooldown = curWeapons[0].FireRate;
            hud = GameObject.FindGameObjectWithTag("UI").GetComponent<HUD>();
            hud.localPlayer = this;

            Cursor.lockState = CursorLockMode.Locked;
            rifleSound = GetComponent<AudioSource>(); // Gets the audio source
        }
    }

    public void Update() //Used to make reference to the sub-routines/methods.
    {
        if (isLocalPlayer) //Checks whether or not the player script is attached to the local player.
        {
            if (!playerDead) //Checks whether not the player is dead.
            {
                if (!isDowned) //Checks whether or not the player is downed.
                {
                    Movement(); //Calls upon the movement function.
                    if (canInteract) //Checks whether or not the player can interact.
                    {
                        CmdInteractions(); //Calls upon the function that allows interactions between the player and other entities.

                        CmdShooting(selectedIndex); //Calls upon the function that allows the player to shoot.

                        if (revivingPlayer == true) //Checks whether or not the player is attempting to revive another player.
                        {
                            RevivingPlayer(); //Calls upon the function that allows the player to revive another.
                        }
                    }
                    WeaponModify(); //Checks whether or not the curWeapon's list is has been modifed or not to allow for perks to apply their affect.
                }
                else
                {
                    PlayerDying(); //Function that shows that the player is downed and dying.
                }
            }
            else
            {
                SpectatorMovement(); //Calls upon the function that allows the player to spectate.
            }
            MouseMovement(); //Allows the player to move and look around.
            hud.weaponIndex = selectedIndex; //Syncs up the selected weapon with the HUD to display ammo.
        }
    }
    #endregion

    #region Movement
    public void Movement() //Controls all player movement.
    {
        moveDirection.z = Input.GetAxis("Vertical");
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection = transform.TransformDirection(moveDirection);
        walkSound = GetComponent<AudioSource>();
        if (moveDirection.x >0 || moveDirection.z >0) //Checks whether or not the player is moving to allow for sound to play.
        {
            walkSound.Play();
        }
        if (IsGrounded()) //Checks if the player is Grounded and applies all Y axis based movement.
        {
            moveDirection.y = 0;
            gravityIncreaseTimer = 0;
            appliedGravity = gravity;
            if (Input.GetButtonDown("Jump")) //Defaults back to default jump.
            {   
                moveDirection.y += jumpSpeed;
                increaseTimer = 0;
            }
        }
        else //If the player isn't grounded, change values for gravity overtime.
        {
            gravityIncreaseTimer += Time.deltaTime;
            if (gravityIncreaseTimer >= 0.4f && appliedGravity <= 50)
            {
                appliedGravity = gravity + (gravityIncreaseTimer * 16f);
            }
        }
        if (Input.GetKey(KeyCode.LeftShift)) //Checks if the player is sprinting and applies extra force.
        {
            moveDirection.x *= (speed * sprintMultiplier);
            moveDirection.z *= (speed * sprintMultiplier);
        }
        else //Applies default speed if not sprinting.
        {
            moveDirection.x *= speed;
            moveDirection.z *= speed;
        }

        moveDirection.y -= appliedGravity * Time.deltaTime; //Applies gravity.
        controller.Move(moveDirection * Time.deltaTime); //Applies movement.
    }
    #endregion

    #region MouseMovement
    public void MouseMovement() //Controls all mouse movement.
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensX, 0); //Records and applies movement for X axis.
        rotationY += Input.GetAxis("Mouse Y") * sensY; //Records movement for Y axis.
        rotationY = Mathf.Clamp(rotationY, minY, maxY); //Applies a Clamp to give boundaries to the movement on the Y axis.
        camera.transform.localEulerAngles = new Vector3(-rotationY, 0, 0); //applies movement for the Y axis.
    }
    #endregion

    #region Spectate
    public void SpectatorMovement() //All the movement tied in with spectating.
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (Input.GetButton("Spectate")) //Checks wther or not the player is wanting to move up or down.
        {
            moveDirection.y = Input.GetAxis("Spectate") * speed;
        }
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection.z *= (speed * 2);
        moveDirection.x *= (speed * 2);
        Debug.Log(moveDirection);
        controller.Move(moveDirection * Time.deltaTime); //Applies movement.
    }
    #endregion

    #region Weapon Modify
    public void WeaponModify() //Used to check whether the current weapon has been modified 
    {
        if (curWeapons[selectedIndex].BeenModified == false) //Checks whehther or not the curWeapon has been modified.
        {
            Debug.Log("Activated");
            for (int i = 0; i < curPerks.Count; i++) //Applies the effect of each perk onto the weapon.
            {
                curPerks[i].ApplyStats(this);
            }
        }
    }
    #endregion

    #region Shooting
    [Command]
    public void CmdShooting(int weaponIndex) //Allows the player to shoot the current equiped weapon.
    {
        Debug.Log("Clip: " + curWeapons[weaponIndex].Clip);
        Debug.Log("Ammo: " + curWeapons[weaponIndex].Ammo);
        if (shotCooldown <= 0) //Checks whether or not the cooldown for shooting has reached 0.
        {
            if (curWeapons[weaponIndex].Clip > 0 && canFire == true) //Checks whether or not the current weapon still has ammo and is able to fire.
            {
                if (Input.GetMouseButtonDown(0)) //Checks whether or not the player is attempting to shoot and calls upon the shoot function from the weapon as well as resetting the cooldown and taking away from the clip.
                {
                    curWeapons[weaponIndex].Shoot(camera.GetComponentInChildren<Camera>(), gameObject, this);
                    shotCooldown = curWeapons[weaponIndex].FireRate;
                    curWeapons[weaponIndex].Clip--;
                    rifleSound.Play();
                    Debug.Log("Gun shot sound");
                }
            }
            else if (curWeapons[weaponIndex].Clip == 0 && curWeapons[weaponIndex].Ammo > 0) //If the clip is empty and there is still ammo, call upon the co-routine that allows the player to reload.
            {
                StartCoroutine(Reload(weaponIndex));
                canFire = false;
            }
        }
        else //Count down for the cooldown.
        {
            shotCooldown -= Time.deltaTime;
        }

    }

    public IEnumerator Reload(int weaponIndex) //Used to reload the gun
    {
        Debug.Log("Reloading: " + curWeapons[weaponIndex].Clip);
        curWeapons[weaponIndex].Ammo -= curWeapons[weaponIndex].ClipSize; //Takes the Clip Size away from the Ammo total and loads it into the clip to allow the player to shoot again.
        curWeapons[weaponIndex].Clip = curWeapons[weaponIndex].ClipSize;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(curWeapons[weaponIndex].ReloadTime); //Waits for the reload time and then allows the player to shoot again.
        canFire = true;
        Debug.Log("Reloaded: " + curWeapons[weaponIndex].Clip);
    }
    #endregion

    #region Interactions
    [Command]
    public void CmdInteractions() //Used to check all interactions between the player and the environment/entities.
    {
        if (Input.GetKeyDown(KeyCode.E)) //Checks whether or not the player is attempting to interact and shoots a raycast out.
        {
            Vector3 mousePosition = camera.GetComponentInChildren<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(mousePosition, camera.transform.forward, out hit, interactRange)) //Checks whether or not the raycast has hit anything.
            {
                if (hit.collider.tag == "Player") //If the raycast hits another player while they are downed, begin reviving.
                {
                    hit.collider.GetComponent<Player>().beingRevived = true;
                    revivingPlayer = true;
                }
                if (hit.collider.tag == "PerkMachine") //If the raycast hits a PerkMachine, check whether or not the player is able to buy said perk and add it to the curPerk list if they can.
                {
                    PerkMachine perkHitRef = hit.collider.GetComponent<PerkMachine>();
                    if (PerkCheck(perkHitRef.Perk) == false) //Checks whether or not the player has already received this perk.
                    {
                        if (money >= perkHitRef.Cost) //Checks if the player has enough money for the perk.
                        {
                            money -= perkHitRef.Cost;
                            curPerks.Add(PerkData.AddPerk(perkHitRef.Perk));
                            curPerks[curPerks.Count - 1].ApplyStats(this);
                        }
                    }
                }
                if (hit.collider.tag == "GunVendor") //If the raycast hits a GunVendor, check whether or not the player does not already have said gun and either allow the player to buy said gun or refill ammo.
                {
                    WeaponVendor weaponHitRef = hit.collider.GetComponent<WeaponVendor>();
                    Debug.Log("Registered Weapon Vendor");
                    if (WeaponCheck(weaponHitRef.WeaponName) == false) //Checks whether or not the player already has the gun.
                    {
                        Debug.Log("Name Check Complete Weapon Vendor");
                        if (money >= weaponHitRef.Cost) //Checks whether or not the player has enough money for the gun and if they do, buys it.
                        {
                            money -= weaponHitRef.Cost;
                            curWeapons.Add(WeaponType.AddWeapon(weaponHitRef.WeaponName));
                        }
                    }
                    else //If the player alreay has the gun, allows the player to buy a ammo refill.
                    {
                        if (money >= weaponHitRef.Cost) //Checks whether or not the player has enough money for the refill.
                        {
                            money -= weaponHitRef.Cost;
                            for (int i = 0; i < curWeapons.Count; i++) //Checks whether or not the weapon is in the current weapon slot and applies the ammo refill.
                            {
                                if (curWeapons[i].Name == weaponHitRef.WeaponName)
                                {
                                    curWeapons[i].Ammo = curWeapons[i].AmmoMax;
                                }
                            }
                        }
                    }
                }
                if (hit.collider.tag == "Door") //If the raycast hits a Door, check whether or not the player has enough money to open said door 
                {
                    Door doorHitRef = hit.collider.GetComponentInParent<Door>();
                    int doorIndex = GetNumberFromString(hit.collider.name);
                    if (doorHitRef.doorOpen[doorIndex] == false) //Checks whether or not the door has already been openned.
                    {
                        if (money >= doorHitRef.cost[doorIndex]) //If the player has enough money for the door, buys the door and opens it.
                        {
                            money -= doorHitRef.cost[doorIndex];
                            doorHitRef.OpenDoor(doorIndex);
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region Health Management
    public void TakeDamage(int damageDealt) //Function that can be called upon to deal damage to the player.
    {
        if (canTakeDamage) //Checks whether or not the player has an iFrame and takes away damage if they don't.
        {
            curHealth -= damageDealt;
            if (curHealth <= 0) //Checks whether or not the player has 0 health to allow the player to go down.
            {
                isDowned = true;
                canTakeDamage = false;
            }
            StartCoroutine(iFrame());
        }
    }

    public IEnumerator iFrame() //Co-routine that's used as a timer to check whether or not the player can take damage.
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.3f);
        canTakeDamage = true;
    }

    public void RevivingPlayer() //Used to call upon the function that allows the player to be revived.
    {
        StartCoroutine(Revived(transform));
    }

    public void PlayerDying() //Used to slowly kill off the player and once the timer runs out, sets up spectate mode for the player.
    {
        timeTillDeath += Time.deltaTime;
        if (timeTillDeath >= deathTime)
        {
            StartCoroutine(SetupSpectator());
        }
    }

    public IEnumerator SetupSpectator() //co-routine used to stop all interactions from the environment with the player and allow them to be classified as dead.
    {
        playerDead = true;
        deaths++;
        GameManager.playersDead.Add(this);
        isDowned = false;
        timeTillDeath = 0;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        Debug.Log("Death Initialized");
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator Revived(Transform spawnPos) //Revives the player and unlocks all abilities for the player.
    {
        gameObject.transform.position = spawnPos.position;
        curHealth = maxHealth;
        playerDead = false;
        isDowned = false;
        canTakeDamage = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos() //Displays the Gizmos for isGrounded.
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.localPosition + groundDistance, groundedOverlay);
    }
    #endregion
}