using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovementControls : MonoBehaviour
{
    //General inputs
    [SerializeField] 
    private float playerSpeed;
    private Rigidbody2D rb2D;

    [SerializeField]
    private float sprintFactor = 1.5f;
    public float sprintDuration = 2f;
    public float sprintTimer;
    private bool isSprinting = false;

    private float horizontalMove = 0;

    //Dash variables
    [SerializeField]
    private float dashSpeed;
    private float dashFactor = 2f;
    private float dashTimer = 0.5f;
    public float dashTime;
    private bool isDashing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialize the rigidbody variables
        rb2D = GetComponent<Rigidbody2D>();

        //Setting the dash time to timer
        dashTime = dashTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //Inputs();
        //DashInputs();    
    }

    //FixedUpdate runs every frame at a set interval 
    //Is good for physics calculations
    private void FixedUpdate()
    {
        //Float variable to store horizontal input
        //Horizontal can be -1, 0, or 1
        float h = Input.GetAxis("Horizontal");

        //Set the movement function
        Move(h);

        //Set dash function
        Dash(h);
    }

    private void Move(float hSpeed)
    {
        //Ternary if statement
        //is the bool is sprinting true? if it is multiply hspeed by playerspeed and sprint factor
        //if it is false (:) multiply hspeed by playerspeed
        float xVelocity = isSprinting ? hSpeed * playerSpeed * sprintFactor : hSpeed * playerSpeed;

        //Assign the left shift key to sprinting
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        //What to do if the player is sprinting
        if (isSprinting) { sprintTimer -= Time.deltaTime; } //Subtract 1 second by delta time
        else { sprintTimer = sprintDuration; } //If player is not sprinting, reset the timer

        //Check to make sure the timer hasn't elapsed
        if(sprintTimer <= 0) { sprintTimer = 0;
            isSprinting = false;
        }

        rb2D.linearVelocity = new Vector2(xVelocity, rb2D.linearVelocity.y);
    }

    private void Dash(float hSpeed)
    {
        Vector2 dashVector = new Vector2(dashSpeed * dashFactor, 0);

        //Assigning dash function to C key
        isDashing = Input.GetKey(KeyCode.C);

        //If player is moving in the positive (1) or negative (-1) direction and presses dash
        //Apply dash speed
        if (hSpeed > 0 && isDashing) { rb2D.linearVelocity = dashVector; }
        else if (hSpeed < 0 && isDashing ) { rb2D.linearVelocity = -dashVector; }
    }


    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.blue);
    }
}
