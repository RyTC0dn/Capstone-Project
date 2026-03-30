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

    public Character character;

    private Player_Controller playerController;
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
    [SerializeField] private float dashTimer = 0.5f;
    public float dashTime;
    private bool isDashing = false;
    private bool isInvincible = false;

    [Header("Sprite Settings")]
    [HideInInspector] public bool isFacingRight = true;

    [Space(10)]
    [Header("Audio")]
    [SerializeField] private float idleTimer; //Timer for idle audio, manually set in inspector

    private bool isIdle = false;
    public AudioPlayer movementAudioPlayer;
    public AudioSource movementAudioSource;

    [SerializeField]
    private float idleAccumTimer = 0f;

    [Header("Movement Audio Check")]
    public int grassMin, grassMax;

    public int stoneMin, stoneMax;
    private bool isGrass, isStone;
    [SerializeField] private LayerMask stoneLayer; //Apply the ground layer
    [SerializeField] private LayerMask grassLayer;
    [SerializeField] private float detectionLength;

    [Header("UI")]
    public GameObject buttonPrompt;

    public GameObject keyPrompt;
    public SceneInfo sceneInfo;
    private bool isController;
    private bool canSave;

    private void Awake()
    {
        ////Enable player controller
        //playerController = PlayerInputHub.controls;
        canSave = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //Initialize the rigidbody variables
        rb2D = GetComponent<Rigidbody2D>();

        //Setting the dash time to timer
        dashTime = dashTimer;

        buttonPrompt.SetActive(false);
        keyPrompt.SetActive(false);

        isController = sceneInfo.OnDeviceChange(Gamepad.current);

        //Keep track of the current scene
        //isGrass = SceneManager.GetActiveScene().name == townName;
        isGrass = Physics2D.Raycast(transform.position, Vector2.down, detectionLength, grassLayer);
        isStone = Physics2D.Raycast(transform.position, Vector2.down, detectionLength, stoneLayer);
    }

    public void InteractEvent()
    {
        bool key = Keyboard.current?.eKey.wasPressedThisFrame ?? false;
        bool button = Gamepad.current?.buttonWest.wasPressedThisFrame ?? false;
        bool isPressed = key || button;

        bool resetKey = Keyboard.current?.pKey.wasPressedThisFrame ?? false;
        bool resetButton = Gamepad.current?.dpad.up.wasPressedThisFrame ?? false;
        bool reset = resetKey || resetButton;

        //If either the ekey or xButton is pressed
        if (isPressed)
        {
            //Send the interact event out
            playerInteract.Raise(this, isPressed);
        }
        if (reset)
        {
            ResetLevel();
        }
    }

    public void ResetLevel()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    //FixedUpdate runs every frame at a set interval
    //Is good for physics calculations
    private void FixedUpdate()
    {
        //Set the movement function
        moveInput.x = Input.GetAxisRaw("Horizontal");

        bool keyInput = Keyboard.current?.eKey.isPressed ?? false;
        bool buttonInput = Gamepad.current?.xButton.isPressed ?? false;

        Move(moveInput.x);
        InteractEvent();
        //Depending on character state
        switch (CharacterSelect.selectCharacter)
        {
            //Play character specific animations
            //animation setup within character 1 animator
            case Character.Knight:
                animator.SetBool("isKnight", true);
                break;

            case Character.Cleric:
                animator.SetBool("isKnight", false);
                break;

            case Character.Huntress:
                //Will implement in future here
                break;

            case Character.Wizard:
                //Will implement in future here
                break;

            default:
                break;
        }

        if ((keyInput || buttonInput) && canSave)
        {
            SavePlayerPosition();
        }

    }

    private void Update()
    {
        //Idle Audio
        if (moveInput.x == 0)
        {
            idleAccumTimer += Time.deltaTime; //Add 1 second by delta time

            if (!isIdle && idleAccumTimer >= idleTimer)
            {
                isIdle = true;
                //Play a random clip from the audio player, using the range of 14 to 16 for the clip index
                if (movementAudioPlayer != null && movementAudioSource != null)
                    //audioPlayer.PlayRandomClip(audioSource, 14, 16);

                    idleAccumTimer = 0f; //Reset the accumulated timer
            }
            //Debug.Log("Player is idle. Accumulated time: " + idleAccumTimer);
        }
        else
        {
            idleAccumTimer = 0f; //Reset the accumulated timer if the player is moving
            if (isIdle)
                isIdle = false; //Reset the idle state if the player starts moving
        }
    }

    private void Move(float h)
    {
        rb2D.linearVelocity = new Vector2(h * playerSpeed, rb2D.linearVelocity.y);

        ///The entire object is flipped based on direction
        ///to ensure that the attack collider will always be in front of the player
        if (h > 0)
        {
            isFacingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (h < 0)
        {
            isFacingRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        //Play walking animation while player is in motion
        animator.SetFloat("horizontal", h);

        if ((h > 0 || h < 0) && isGrass)
        {
            movementAudioPlayer.PlayAudioCycle(movementAudioSource,
                grassMin, grassMax);
        }
        else if ((h > 0 || h < 0) && isStone)
        {
            movementAudioPlayer.PlayAudioCycle(movementAudioSource,
                stoneMin, stoneMax);
        }

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
        if (dashTime > 0)
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
        if (collision.CompareTag("Elevator"))
        {
            //Check for if the controller is connected
            if (isController)
            {
                buttonPrompt.SetActive(true);
            }
            else
            {
                keyPrompt.SetActive(true);
            }
        }
        if (collision.CompareTag("Ladder"))
        {
            animator.SetBool("isClimbing", true);
        }
        if (collision.CompareTag("SaveStation"))
        {
            canSave = true;
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
        if (collision.CompareTag("Elevator"))
        {
            //Check for if the controller is connected
            if (isController)
            {
                buttonPrompt.SetActive(false);
            }
            else
            {
                keyPrompt.SetActive(false);
            }
        }
        if (collision.CompareTag("Ladder"))
        {
            animator.SetBool("isClimbing", false);
        }
        if (collision.CompareTag("SaveStation"))
        {
            canSave = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.blue);
    }

    public void SavePlayerPosition()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadPlayerPosition()
    {
        PlayerControllerData data = SaveSystem.LoadPlayer();

        Vector2 position;
        position.x = data.position[0];
        position.y = data.position[1];
        transform.position = position;
    }
}