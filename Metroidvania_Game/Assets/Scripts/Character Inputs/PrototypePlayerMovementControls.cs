using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PrototypePlayerMovementControls : MonoBehaviour
{
    [Header("General input variables")]

    public static PrototypePlayerMovementControls Instance { get; private set; }

    [SerializeField] 
    private float playerSpeed;
    private Rigidbody2D rb2D;
    public Transform playerSpawnPoint;

    public float hSpeed;

    [SerializeField]
    private float sprintFactor = 1.5f;
    public float sprintDuration = 2f;
    public float sprintTimer;
    private bool isSprinting = false;

    private float horizontalMove = 0;

    public bool gotHit;

    [Header("Dash Settings")]

    private float dashSpeed;
    private float dashFactor = 2f;
    [SerializeField]private float dashTimer = 0.5f;
    public float dashTime;
    private bool isDashing = false;

    [Header("Sprite Settings")]    
    private SpriteRenderer knightSP;
    [HideInInspector] public bool isFacingRight = true;

    [Header("UI Settings")]
    UIManager ui;

    PrototypeShop shop;
    public SceneChanger sceneChanger;
    GameManager gm;
    PrototypePlayerAttack playerAttack;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialize the rigidbody variables
        rb2D = GetComponent<Rigidbody2D>();

        //Setting the dash time to timer
        dashTime = dashTimer;

        //Initialize the spriterenderer for the player
        knightSP = GetComponent<SpriteRenderer>();

        //Initilize the UI manager
        ui = FindAnyObjectByType<UIManager>();

        shop = FindAnyObjectByType<PrototypeShop>();

        gm = FindAnyObjectByType<GameManager>();

        playerAttack = GetComponent<PrototypePlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        ///Currently have the shop being called in the player controller script, may move elsewhere
        if(Keyboard.current.eKey.isPressed && shop.isNearShop)
        {
            shop.EnableShop();
            gm.StateSwitch(GameStates.Pause);
            playerAttack.enabled = false;
        }
    }

    //FixedUpdate runs every frame at a set interval 
    //Is good for physics calculations
    private void FixedUpdate()
    {
        //Float variable to store horizontal input
        //Horizontal can be -1, 0, or 1
        hSpeed = Input.GetAxisRaw("Horizontal");

        //Set the movement function
        Move(hSpeed);

        if (dashTime > 0)
        {
            //Set dash function
            Dash(hSpeed);
        }

    }

    private void Move(float hSpeed)
    {
        //Assigning booleans to the key inputs
        float movement = Input.GetAxisRaw("Horizontal");

        //Ternary if statement
        //is the bool is sprinting true? if it is multiply hspeed by playerspeed and sprint factor
        //if it is false (:) multiply hspeed by playerspeed
        float xVelocity = isSprinting ? hSpeed * playerSpeed * sprintFactor : hSpeed * playerSpeed;

        //Assign the left shift key to sprinting
        isSprinting = Keyboard.current.leftShiftKey.isPressed;

        //What to do if the player is sprinting
        if (isSprinting) { sprintTimer -= Time.deltaTime; } //Subtract 1 second by delta time
        else { sprintTimer = sprintDuration; } //If player is not sprinting, reset the timer

        //Check to make sure the timer hasn't elapsed
        if (sprintTimer <= 0)
        {
            sprintTimer = 0;
            isSprinting = false;
        }

        rb2D.linearVelocity = new Vector2(xVelocity, rb2D.linearVelocity.y);

        if(movement > 0 )
        {
            isFacingRight = true;
            knightSP.flipX = true;           
        }
        if(movement < 0 )
        {
            isFacingRight = false;
            knightSP.flipX = false;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the player walks into a coin
        if (collision.CompareTag("Currency"))
        {
            GameManager.instance.coinTracker++;
            ui.CoinsCollected();
            gm.PlayCoinAudio();
            Destroy(collision.gameObject);
        }

        //Check if the player walks into trap obkects
        if (collision.CompareTag("Traps"))
        {
            GameManager.instance.playerLives--;
            ui.UpdateUI();
            transform.position = playerSpawnPoint.position;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.blue);
    }
}
