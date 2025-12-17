using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;

public class PlayerJumps : MonoBehaviour
{
    //Jump force 
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;     //How muc to increase gravity scale when falling 
    public float lowJumpMultiplier = 2f;   //Factor for small jumps
    public LayerMask groundLayer;         //Layer for the ground
    [SerializeField]private float gravityScale = 4f;

    ////Variable for the animator
    //private Animator jumpSP;

    //Boolean to store if the player is on the ground or not
    public bool isGrounded = false;

    private Rigidbody2D rb2d;
    private Animator animator;

    //Coyote time 
    public float coyoteTime;
    public float coyoteTimeMax = 0.2f;
    private float raycastLength = 2;

    public Player_Controller controller;

    private void Awake()
    {
        ////Enable player controller
        //controller = PlayerInputHub.controls;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the rigidbody
        rb2d = GetComponent<Rigidbody2D>();

        ////Initialize the animator
        //jumpSP = GetComponent<Animator>();

        coyoteTime = coyoteTimeMax; //Set coyote time to the max

        rb2d.gravityScale = gravityScale;

        animator = GetComponent<Animator>();

        
    }

    // Update is called once per frame
    void Update()
    {
        //Check every frame to see if the player is grounded or not
        //Raycasts are lines that check for collisions. They return a true or false 
        //Minimum parameters are astarting point, direction, and size
        //The additional parameter is the layermask, "which layer?"
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, groundLayer);

        //Gravity modifications 
        //If the player is falling, increase gravity factor
        if (!isGrounded)
        {
            //Add the fall multiplier to velocity 
            //Vector2.up * whatever gravity is * fall multiplier - 1
            //and then scale by delta time to account for frame jitter
            fallMultiplier += Time.deltaTime;
            rb2d.linearVelocity += (Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        }
        else if (rb2d.linearVelocity.y > 0)
        {
            rb2d.linearVelocity += (Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
        }

        //Coyote time on/off
        //If the player is on the ground, reset the coyote timer
        //If the player is not on the ground, start subtracting time
        if (isGrounded)
        {
            coyoteTime = coyoteTimeMax;
            fallMultiplier = 2.5f;
        }
        else
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, rb2d.linearVelocity.y);
            coyoteTime -= Time.deltaTime;
        }

        OnJump();
    }

    public void OnJump()
    {
        bool key = Keyboard.current?.spaceKey.wasPressedThisFrame ?? false;
        bool button = Gamepad.current?.buttonSouth.wasPressedThisFrame ?? false;
        bool isPressed = key || button;

        //Jump input 
        if (isPressed && coyoteTime > 0)
        {
            //Trigger animation before the jump executes
            animator.SetTrigger("isJumping");

            //Set the rigidbody's velocity to whatever its current velocity is on x
            //and jump force on y
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpForce);

            coyoteTime = 0;
            isGrounded = false;            
        }
        
    }


    //Special function to make gizmos visible 
    //On draw gizmos selected will show gizmos when you select the game object
    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector2.down * raycastLength, Color.blue);
    }
}
