using System.Collections;
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

    Player_Controller playerController;
    [HideInInspector] public Vector2 moveInput;
     
    public float playerSpeed;
    [HideInInspector] public float horizontalSpeed;
    private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;

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
    private bool isInvincible = false;

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

    private void Awake()
    {
        playerController = new Player_Controller();
        playerController.Enable();

        //Subscribing to the move event
        playerController.Gameplay.Movement.performed += OnMove;
        playerController.Gameplay.Movement.canceled += OnMove;
        playerController.Gameplay.Interact.performed += InteractEvent;
        playerController.Gameplay.Interact.canceled += InteractEvent;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialize the rigidbody variables
        rb2D = GetComponent<Rigidbody2D>();

        //Setting the dash time to timer
        dashTime = dashTimer;

        shop = FindAnyObjectByType<PrototypeShop>();

        playerAttack = FindAnyObjectByType<PrototypePlayerAttack>();

        enemyAttack = FindAnyObjectByType<BasicEnemyAttackState>();
    }

    private void OnDestroy()
    {
        playerController.Gameplay.Movement.performed -= OnMove;
        playerController.Gameplay.Movement.canceled -= OnMove;
        playerController.Gameplay.Interact.performed -= InteractEvent;
        playerController.Gameplay.Interact.canceled -= InteractEvent;
    }

    public void InteractEvent(InputAction.CallbackContext context)
    {
        //If either the ekey or xButton is pressed
        if (context.performed)
        {
            //Send the interact event out
            playerAttack.enabled = false;
            playerInteract.Raise(this, context.performed);
        }
    }

    //FixedUpdate runs every frame at a set interval 
    //Is good for physics calculations
    private void FixedUpdate()
    {

        //Set the movement function
        Move();

        if(Keyboard.current.rKey.isPressed)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void Move()
    {
        float h = moveInput.x;

        rb2D.linearVelocity = new Vector2(h * playerSpeed, rb2D.linearVelocity.y);

         ///The entire object is flipped based on direction
        ///to ensure that the attack collider will always be in front of the player
        if(h > 0)
        {
            isFacingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(h < 0)
        {
            isFacingRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        animator.SetBool("isRunning", h != 0);

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
    }

    //Only call this function when the player lives equal 0
    

    private void Dash(float hSpeed)
    { 
        if(dashTime > 0)
        {
            dashSpeed = playerSpeed;
            Vector2 dashVector = new Vector2(dashSpeed * dashFactor, 0);

            //Assigning dash function to C key
            isDashing = Keyboard.current.leftShiftKey.isPressed;

            //If player is moving in the positive (1) or negative (-1) direction and presses dash
            //Apply dash speed
            if (hSpeed > 0 && isDashing) { rb2D.linearVelocity = dashVector; dashTime -= Time.deltaTime; }
            else if (hSpeed < 0 && isDashing) { rb2D.linearVelocity = -dashVector; dashTime -= Time.deltaTime; }

            PlayerHealth hp = GetComponent<PlayerHealth>();
            hp.isInvulnerable = true;
        }
        else
        {
            PlayerHealth hp = GetComponent<PlayerHealth>();
            hp.isInvulnerable = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.blue);
    }
}
