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
    [Space(10)]

    [Header("Idle Audio")]
    [SerializeField] private float idleTimer; //Timer for idle audio, manually set in inspector
    private bool isIdle = false;
    AudioPlayer audioPlayer;
    AudioSource audioSource;
    [SerializeField] private float idleAccumTimer = 0f;  

    private void Awake()
    {
        ////Enable player controller
        //playerController = PlayerInputHub.controls;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialize the rigidbody variables
        rb2D = GetComponent<Rigidbody2D>();
        audioPlayer = GetComponentInChildren<AudioPlayer>();
        audioSource = GetComponent<AudioSource>();

        //Setting the dash time to timer
        dashTime = dashTimer;
    }

    public void InteractEvent()
    {
        bool key = Keyboard.current?.eKey.wasPressedThisFrame ?? false;
        bool button = Gamepad.current?.buttonWest.wasPressedThisFrame ?? false;
        bool isPressed = key || button; 

        //If either the ekey or xButton is pressed
        if (isPressed)
        {
            //Send the interact event out
            playerInteract.Raise(this, isPressed);
        }
    }

    //FixedUpdate runs every frame at a set interval 
    //Is good for physics calculations
    private void FixedUpdate()
    {

        //Set the movement function
        moveInput.x = Input.GetAxisRaw("Horizontal");

        Move(moveInput.x);
        InteractEvent();
    }

    private void Update()
    {
        //Idle Audio
        if (moveInput.x == 0)
        {
            idleAccumTimer += Time.deltaTime; //Add 1 second by delta time

            if(!isIdle && idleAccumTimer >= idleTimer)
            {
                isIdle = true;
                //Play a random clip from the audio player, using the range of 14 to 16 for the clip index
                if (audioPlayer != null && audioSource != null)
                    audioPlayer.PlayRandomClip(audioSource, 14, 16);

                idleAccumTimer = 0f; //Reset the accumulated timer
            }
            Debug.Log("Player is idle. Accumulated time: " + idleAccumTimer);
        }
        else
        {
            idleAccumTimer = 0f; //Reset the accumulated timer if the player is moving
            if(isIdle)
                isIdle = false; //Reset the idle state if the player starts moving
        }
    }



    private void Move(float h)
    {

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

        //animator.SetBool("isRunning", h != 0);
        animator.SetFloat("horizontal", h);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            animator.SetBool("isClimbing", true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            animator.SetBool("isClimbing", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            animator.SetBool("isClimbing", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.blue);
    }
}
