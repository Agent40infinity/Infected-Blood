using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------
 * Script Created by: Aiden Nathan.
 * Frankensteined camera movement from Jaymie.
 -------------------------------------------------------------------------*/

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    #region Variables
    //Movement:
    public Vector3 moveDirection; //Vector3 used to store the movement values.
    public float jumpSpeed = 12; //speed applied to jumping.
    public float speed = 10; //speed applied for default movement.
    public float sprintSpeed = 15; //speed applied when sprinting.
    public float gravity = 20; //default return value for gravity.
    public float appliedGravity; //gravity used to affect the player.
    public float gravityIncreaseTimer; //Timer used to increase appliedGravity's effect over time.
    public int teleportPoint = 3; //Value used to check if the player is below a certain point so that they can be teleported.

    public float increaseTimer = 0; //Counter used to disable the players ability to increase the sprintSpeed after a certain time frame.
    public bool canIncrease = false; //Determines whether or not player is able to increase their sprintSpeed.
    public static bool canMove = true; //Gateway variable to lock all movement (Camera and Player).
    public static bool freezeMove = false; //Gateway variable to freeze all Player movement.

    public Vector3 groundedOverlay = new Vector3(0.2f, 0.1f, 0.2f); //Vector3 used to store the values for the collider used to check for isGrounded.
    public Vector3 groundDistance = new Vector3(0f, -1f, 0f); //Vector3 used to store the position of the isGrounded check.

    public List<Weapon> curWeapons = new List<Weapon>();

    //MouseMovement: 
    [Range(0, 20)]
    public float sensX = 15; //Sensitivity for X axis of mouse.
    [Range(0, 20)]
    public float sensY = 15; //Sensitivity for Y axis of mouse.

    public float minY = -65; //Max value for the rotation on the Y axis for the camera.
    public float maxY = 65; //Min value for the rotation on the Y axis for the camera.
    public float rotationY = 0; //value to store the Y axis for the rotation.

    //References:
    private CharacterController controller; //Reference for the attached character controller.
    public GameObject camera; //Reference for the attached camera.
    public LayerMask ground; //Reference for the LayerMask that stores the value for ground.

    public bool isGrounded() //Used to determine if the player is grounded based on a collider check.
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.localPosition + groundDistance, groundedOverlay, Quaternion.identity, ground); //Creates a overlap box to check whether the player is grounded.
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.layer == ground) //Checks each gameObject that the collider hits to see if it is layered as "ground".
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
        canMove = true;
        controller = gameObject.GetComponent<CharacterController>();
        camera = GameObject.FindGameObjectWithTag("PlayerHead");
    }

    public void Update() //Used to make reference to the sub-routines/methods.
    {
        Debug.Log(isGrounded());
        if (canMove)
        {
            Movement();
            MouseMovement();
        }
    }
    #endregion

    #region Movement
    public void Movement() //Controls all player movement.
    {
        moveDirection.z = Input.GetAxis("Vertical");
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection = transform.TransformDirection(moveDirection);
        if (isGrounded()) //Checks if the player is Grounded and applies all Y axis based movement.
        {
            moveDirection.y = 0;
            gravityIncreaseTimer = 0;
            appliedGravity = gravity;   
            if (Input.GetButtonDown("Jump")) //Defaults back to default jump.
            {
                moveDirection.y += jumpSpeed;
                increaseTimer = 0;
                canIncrease = true;
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
            moveDirection.x *= sprintSpeed;
            moveDirection.z *= sprintSpeed;
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

    #region Gizmos
    private void OnDrawGizmos() //Displays the Gizmos for isGrounded.
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.localPosition + groundDistance, groundedOverlay);
    }
    #endregion
}