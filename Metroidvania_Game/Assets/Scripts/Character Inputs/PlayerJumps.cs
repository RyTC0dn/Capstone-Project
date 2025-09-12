using UnityEngine;

public class PlayerJumps : MonoBehaviour
{
    //Jump force 
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;     //How muc to increase gravity scale when falling 
    public float lowJumpMultiplier = 2f;   //Factor for small jumps
    public LayerMask groundLayer;         //Layer for the ground

    ////Variable for the animator
    //private Animator jumpSP;

    //Boolean to store if the player is on the ground or not
    public bool isGrounded = false;

    private Rigidbody2D rb2d;

    //Coyote time 
    public float coyoteTime;
    public float coyoteTimeMax = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the rigidbody
        rb2d = GetComponent<Rigidbody2D>();

        ////Initialize the animator
        //jumpSP = GetComponent<Animator>();

        coyoteTime = coyoteTimeMax; //Set coyote time to the max
    }

    // Update is called once per frame
    void Update()
    {
        //Check every frame to see if the player is grounded or not
        //Raycasts are lines that check for collisions. They return a true or false 
        //Minimum parameters are astarting point, direction, and size
        //The additional parameter is the layermask, "which layer?"
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        //Jump input 
        if (Input.GetKeyDown(KeyCode.Space) && coyoteTime > 0)
        {
            //Set the rigidbody's velocity to whatever its current velocity is on x
            //and jump force on y
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpForce);
        }

        //Gravity modifications 
        //If the player is falling, increase gravity factor
        if (rb2d.linearVelocity.y < 0)
        {
            //Add the fall multiplier to velocity 
            //Vector2.up * whatever gravity is * fall multiplier - 1
            //and then scale by delta time to account for frame jitter
            rb2d.linearVelocity += (Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        }
        else if (rb2d.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb2d.linearVelocity += (Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
        }

        //Coyote time on/off
        //If the player is on the ground, reset the coyote timer
        //If the player is not on the ground, start subtracting time
        if (isGrounded)
        {
            coyoteTime = coyoteTimeMax;
        }
        else
        {
            coyoteTime -= Time.deltaTime;
        }

        ////Set the animator variable is jumping to the opposite of is grounded
        //jumpSP.SetBool("isJumping", !isGrounded);
    }

    //Special function to make gizmos visible 
    //On draw gizmos selected will show gizmos when you select the game object
    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.blue);
    }
}
