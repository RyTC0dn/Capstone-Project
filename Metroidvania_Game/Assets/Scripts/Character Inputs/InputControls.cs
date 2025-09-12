using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public class InputControls : MonoBehaviour
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
    public float dashTimer = 0.5f;
    private bool canDash = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialize the rigidbody variables
        rb2D = GetComponent<Rigidbody2D>();
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


    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.blue);
    }

    //private void DashInputs()
    //{
    //    Vector3 dash = new Vector3(dashSpeed, 0);
    //    if(Input.GetKey(KeyCode.LeftShift))
    //    {
    //        transform.Translate(dash);
    //    }
    //}

    //private IEnumerator Dash()
    //{
    //    canDash = false;
    //    isDashing = true;
    //    float originalGravity = rb2D.gravityScale;
    //    rb2D.gravityScale = 0;
    //    rb2D.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);
    //    yield return new WaitForSeconds(dashTime);
    //    rb2D.gravityScale = originalGravity;
    //    isDashing = false;
    //    canDash = true;
    //}
}
