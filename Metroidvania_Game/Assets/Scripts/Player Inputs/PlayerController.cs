using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField]private float walkSpeed = 1.0f;
    [Space(20)]

    [Header("Jump Settings")]
    [SerializeField]private float jumpForce = 45f;
    private int jumpBufferCounter = 0;
    [SerializeField] private int jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;
    [Space(20)]

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb2D;
    private float xAxis;
    private Animator animator;

    public static PlayerController instance;
    PlayerStateList pState;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        pState = GetComponent<PlayerStateList>();
    }

    // Update is called once per frame
    void Update()
    {    
        GetInput();
        UpdateJumpVariable();
        Jump();
    }

    #region Horizontal Movement
    void GetInput()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        Movement(xAxis);
    }

    private void Movement(float input)
    {
        //General movement input of player in horizontal axis
        rb2D.linearVelocity = new Vector2(walkSpeed * input, rb2D.linearVelocity.y);

        //Check if the player is facing right or left
        if(input > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(input < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        animator.SetFloat("horizontal", input);
    }
    #endregion

    #region Jump Functions
    void Jump()
    {
        if(Keyboard.current.spaceKey.wasReleasedThisFrame && rb2D.linearVelocity.y > 0)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, 0);

            pState.jumping = false;
        }

        if(!pState.jumping)
        {
            if (jumpBufferCounter > 0 && coyoteTime > 0)
            {
                rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);

                pState.jumping = true;
            }
        }        

        animator.SetTrigger("isJumping");
    }

    public bool IsGrounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, groundLayer) || 
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, groundLayer) || 
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void UpdateJumpVariable()
    {
        if(IsGrounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTime -= Time.deltaTime;
        }

        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter--;
        }
    }
    #endregion
}
