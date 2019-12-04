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
    public int score = 0;
    public int money = 0;
    public int downs = 0;
    public int kills = 0;
    public int deaths = 0;

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
    public List<Weapon> curWeapons = new List<Weapon>();
    public float shotCooldown;
    public bool canFire = true;
    public int selectedIndex = 0;

    //Perk Management:
    [Header("Perk Management")]
    public List<Perks> curPerks = new List<Perks>();

    //Interactions:
    [Header("Interactions")]
    public float interactRange = 10f;
    public bool revivingPlayer = false;
    public bool canInteract = false;

    //Health Management:
    [Header("Health Management")]
    public int curHealth; //Current health of the player.
    public int maxHealth = 100; //Max health of the player.
    public bool isDowned = false; //Checks whether or not the player is downed and needs to be revived.
    public bool beingRevived = false; //Checks whether or not the player is being revived
    public bool playerDead = false;
    public float timeTillDeath = 0;
    public float deathTime = 15f;
    public bool canTakeDamage = true;

    //References:
    [Header("References")]
    public CharacterController controller; //Reference for the attached character controller.
    public GameObject camera; //Reference for the attached camera.
    public GameObject hand;
    public LayerMask ground; //Reference for the LayerMask that stores the value for ground.
    public HUD hud;

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

    bool PerkCheck(PerkType perkInput)
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

    bool WeaponCheck(string weaponInput)
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
        if (isLocalPlayer)
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
        //Debug.Log(IsGrounded());
        if (isLocalPlayer)
        {
            if (!playerDead)
            {
                if (!isDowned)
                {
                    Movement();
                    if (canInteract)
                    {
                        CmdInteractions();

                        CmdShooting(selectedIndex);

                        if (revivingPlayer == true)
                        {
                            RevivingPlayer();
                        }
                    }
                }
                else
                {
                    PlayerDying();
                }
            }
            else
            {
                SpectatorMovement();
            }
            MouseMovement();
            hud.weaponIndex = selectedIndex;
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
        if (moveDirection.x >0 || moveDirection.z >0)
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

    public void SpectatorMovement()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (Input.GetButton("Spectate"))
        {
            moveDirection.y = Input.GetAxis("Spectate") * speed;
        }
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection.z *= (speed * 2);
        moveDirection.x *= (speed * 2);
        Debug.Log(moveDirection);
        controller.Move(moveDirection * Time.deltaTime); //Applies movement.
    }

    #region Shooting
    [Command]
    public void CmdShooting(int weaponIndex)
    {
        Debug.Log("Clip: " + curWeapons[weaponIndex].Clip);
        Debug.Log("Ammo: " + curWeapons[weaponIndex].Ammo);
        if (shotCooldown <= 0)
        {
            if (curWeapons[weaponIndex].Clip > 0 && canFire == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    curWeapons[weaponIndex].Shoot(camera.GetComponentInChildren<Camera>(), gameObject, this);
                    shotCooldown = curWeapons[weaponIndex].FireRate;
                    curWeapons[weaponIndex].Clip--;
                    rifleSound.Play();
                    Debug.Log("Gun shot sound");
                }
            }
            else if (curWeapons[weaponIndex].Clip == 0 && curWeapons[weaponIndex].Ammo > 0)
            {
                StartCoroutine(Reload(weaponIndex));
                canFire = false;
            }
        }
        else
        {
            shotCooldown -= Time.deltaTime;
        }

    }

    public IEnumerator Reload(int weaponIndex)
    {
        Debug.Log("Reloading: " + curWeapons[weaponIndex].Clip);
        curWeapons[weaponIndex].Ammo -= curWeapons[weaponIndex].ClipSize;
        curWeapons[weaponIndex].Clip = curWeapons[weaponIndex].ClipSize;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(curWeapons[weaponIndex].ReloadTime);
        canFire = true;
        Debug.Log("Reloaded: " + curWeapons[weaponIndex].Clip);
    }
    #endregion

    #region Interactions
    [Command]
    public void CmdInteractions()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 mousePosition = camera.GetComponentInChildren<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(mousePosition, camera.transform.forward, out hit, interactRange))
            {
                if (hit.collider.tag == "Player")
                {
                    hit.collider.GetComponent<Player>().beingRevived = true;
                    revivingPlayer = true;
                }
                if (hit.collider.tag == "PerkMachine")
                {
                    PerkMachine perkHitRef = hit.collider.GetComponent<PerkMachine>();
                    if (PerkCheck(perkHitRef.Perk) == false)
                    {
                        if (money >= perkHitRef.Cost)
                        {
                            money -= perkHitRef.Cost;
                            curPerks.Add(PerkData.AddPerk(perkHitRef.Perk));
                        }
                    }
                }
                if (hit.collider.tag == "GunVendor")
                {
                    WeaponVendor weaponHitRef = hit.collider.GetComponent<WeaponVendor>();
                    Debug.Log("Registered Weapon Vendor");
                    if (WeaponCheck(weaponHitRef.WeaponName) == false)
                    {
                        Debug.Log("Name Check Complete Weapon Vendor");
                        if (money >= weaponHitRef.Cost)
                        {
                            money -= weaponHitRef.Cost;
                            curWeapons.Add(WeaponType.AddWeapon(weaponHitRef.WeaponName));
                        }
                    }
                    else
                    {
                        if (money >= weaponHitRef.Cost)
                        {
                            money -= weaponHitRef.Cost;
                            for (int i = 0; i < curWeapons.Count; i++)
                            {
                                if (curWeapons[i].Name == weaponHitRef.WeaponName)
                                {
                                    curWeapons[i].Ammo = curWeapons[i].AmmoMax;
                                }
                            }
                        }
                    }
                }
                if (hit.collider.tag == "Door")
                {
                    Door doorHitRef = hit.collider.GetComponentInParent<Door>();
                    int doorIndex = GetNumberFromString(hit.collider.name);
                    if (doorHitRef.doorOpen[doorIndex] == false)
                    {
                        if (money >= doorHitRef.cost[doorIndex])
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
    public void TakeDamage(int damageDealt)
    {
        if (canTakeDamage)
        {
            curHealth -= damageDealt;
            if (curHealth <= 0)
            {
                isDowned = true;
                canTakeDamage = false;
            }
            StartCoroutine(iFrame());
        }
    }

    public IEnumerator iFrame()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.3f);
        canTakeDamage = true;
    }

    public void RevivingPlayer()
    {
        StartCoroutine(Revived(transform));
    }

    public void PlayerDying()
    {
        timeTillDeath += Time.deltaTime;
        if (timeTillDeath >= deathTime)
        {
            StartCoroutine(SetupSpectator());
        }
    }

    public IEnumerator SetupSpectator()
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

    public IEnumerator Revived(Transform spawnPos)
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