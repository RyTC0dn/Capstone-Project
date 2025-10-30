using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is in need of revision
/// </summary>

public class PrototypePlayerMovementControls : MonoBehaviour
{
    [Header("General input variables")]
    public GameEvent playerInteract;
     
    public float playerSpeed;
    [HideInInspector] public float horizontalSpeed;
    private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;

    public Quaternion playerRot; //For rotating the whole object instead of the sprite

    [SerializeField]
    private float sprintFactor = 1.5f;
    public float sprintDuration = 2f;
    public float sprintTimer;
    private bool isSprinting = false;

    [Header("Dash Settings")]

    private float dashSpeed;
    private float dashFactor = 2f;
    [SerializeField]private float dashTimer = 0.5f;
    public float dashTime;
    private bool isDashing = false;

    [Header("Sprite Settings")]    
    [HideInInspector] public bool isFacingRight = true;

    [Header("UI Settings")]
    PrototypeShop shop;
    PrototypePlayerAttack playerAttack;
    BasicEnemyAttackState enemyAttack;

    [Header("Knockback")]
    public float kbForce = 10f;
    public float kbDuration = 0.2f;
    private bool isKnockedBack = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialize the rigidbody variables
        rb2D = GetComponent<Rigidbody2D>();

        //Setting the dash time to timer
        dashTime = dashTimer;

        shop = FindAnyObjectByType<PrototypeShop>();

        playerAttack = GetComponent<PrototypePlayerAttack>();

        enemyAttack = FindAnyObjectByType<BasicEnemyAttackState>();
    }

    public void InteractEvent()
    {
        //Activate interact event 
        bool pressKey = Keyboard.current.eKey.isPressed;
        bool pressButton = Gamepad.current?.xButton.isPressed ?? false;

        bool isPressed = pressKey || pressButton;

        //If either the ekey or xButton is pressed
        if (isPressed )
        {
            //Send the interact event out
            playerInteract.Raise(this, isPressed);
        }
    }

    //FixedUpdate runs every frame at a set interval 
    //Is good for physics calculations
    private void FixedUpdate()
    {
        //Float variable to store horizontal input
        //Horizontal can be -1, 0, or 1
        float hSpeed = Input.GetAxisRaw("Horizontal");

        //Set the movement function
        Move(hSpeed);

        //if(dashTime > 0)
        //{
        //    //Set dash function
        //    Dash(hSpeed);
        //}


        InteractEvent();
    }

    private void Move(float hSpeed)
    {
        //Assigning booleans to the key inputs
        float movement = Input.GetAxisRaw("Horizontal");

        ////Ternary if statement
        ////is the bool is sprinting true? if it is multiply hspeed by playerspeed and sprint factor
        ////if it is false (:) multiply hspeed by playerspeed
        //float xVelocity = isSprinting ? hSpeed * playerSpeed * sprintFactor : hSpeed * playerSpeed;

        ////Assign the left shift key to sprinting
        //isSprinting = Input.GetKey(KeyCode.LeftShift);

        ////What to do if the player is sprinting
        //if (isSprinting) { sprintTimer -= Time.deltaTime; } //Subtract 1 second by delta time
        //else { sprintTimer = sprintDuration; } //If player is not sprinting, reset the timer

        ////Check to make sure the timer hasn't elapsed
        //if(sprintTimer <= 0) { sprintTimer = 0;
        //    isSprinting = false;
        //}

        rb2D.linearVelocity = new Vector2(hSpeed * playerSpeed, rb2D.linearVelocity.y);

        ///The entire object is flipped based on direction
        ///to ensure that the attack collider will always be in front of the player
        if (movement > 0 )
        {
            isFacingRight = true;
            playerRot = Quaternion.Euler(0, 0, 0);  
            transform.rotation = playerRot;
        }
        if(movement < 0 )
        {
            isFacingRight = false;
            playerRot = Quaternion.Euler(0, 180, 0);
            transform.rotation = playerRot;
        }

       if (movement != 0) //if the player is moving set running else idle animation
      {
           animator.SetBool("isRunning", true);
       }
       else
       {
            animator.SetBool("isRunning", false);
       }
    }

    //Only call this function when the player lives equal 0
    

    private void Dash(float hSpeed)
    {
        while(dashTime > 0)
        {
            dashSpeed = playerSpeed;
            Vector2 dashVector = new Vector2(dashSpeed * dashFactor, 0);

            //Assigning dash function to C key
            isDashing = Keyboard.current.leftShiftKey.isPressed;

            //If player is moving in the positive (1) or negative (-1) direction and presses dash
            //Apply dash speed
            if (hSpeed > 0 && isDashing) { rb2D.linearVelocity = dashVector; dashTime -= Time.deltaTime; }
            else if (hSpeed < 0 && isDashing) { rb2D.linearVelocity = -dashVector; dashTime -= Time.deltaTime; }
        }       
    }

    public void OnTeleportPlayer(Component sender, object data)
    {
        if (data is string elevatorName)
        {
            ElevatorManager.instance.TeleportPlayer(elevatorName, transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.blue);
    }
}
