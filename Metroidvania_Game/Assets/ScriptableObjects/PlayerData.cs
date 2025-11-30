using UnityEngine;
/// <summary>
/// This script was taken from https://github.com/DawnosaurDev/platformer-movement/blob/main/Scripts/PlayerData.cs
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{

    [Header("Gravity")]
    [HideInInspector] public float gravityStrength; //Downward force needed for desired jump height and jump time to apex
    [HideInInspector] public float gravityScale; //Strength of the player's gravity as a multiplier set in physics 2d

    [Space(5)]
    public float fallGravityMult; //Multiplier to the player's grevity scale when falling
    public float maxFallSpeed; //Maximum fall speed of the player when falling
    [Space(5)]
    public float fastFallGravityMult; //Larger multiplier to the player's greavity scale when falling on downward input

    public float maxFastFallSpeed; //Maximum fall speed of the player when performing fast fall

    [Space(20)]

    public float runMaxSpeed; //Target speed we want the player to reach.
    public float runAcceleration; //The speed at which our player accelerates to max speed
    [HideInInspector]public float runAccelAmount; //The actual force multiplied with speedDifferance, applied to the player.
    public float runDecceleration; //The speed at which our player deccelerates fromm their current speed, can be set to runmaxspeed for instant decceleration
    [HideInInspector]public float runDeccelAmount;//Actual force applied
    [Space(5)]
    [Range(0, 1)] public float accelInAir; //Mulipliers applied to acceleration rate when airborne.
    [Range(0, 1)] public float deccelInAir;
    [Space(5)]
    public bool doConserveMomentum = true;

    [Space(20)]

    [Header("Jump")]
    public float jumpHeight; //Height of the player's jump
    public float jumpTimeToApex; //Time between the jump force and reaching the desired jump height. 
    [HideInInspector] public float jumpForce; //The actual force applied on the player when they jump

    [Space(20)]

    [Header("Both Jumps")]
    public float jumpCutGravityMult; //Multiplier to increase gravity if the player releases this jump button while still jumping
    [Range(0, 1)] public float jumpHangeGravityMult; //Reduces gravity while close to the apex or desire max height of the jump
    public float jumpHangTimeThreshold; //Speed, close to 0, where the player will experience extra jump hang.
                                        //The player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)
    [Space(0.5f)]
    public float jumpHangeAccelerationMult;
    public float jumpHangMaxSpeedMult;

    [Header("Wall Jump")]
    public Vector2 wallJumpForce; //The actual force, set by us, applied to the player when wall jumping
    [Space(5)]
    [Range(0, 1)] public float wallJumpRunLerp; //Reduces the effect of player's movement when wall jumping.
    [Range(0, 1.5f)] public float wallJumpTime; //Time after wall jumping the player's movement is slowed for.
    public bool doTurnOnWallJump; //Player will rotate to face wall jumping direction

    [Space(20)]

    [Header("Slide")]
    public float slideSpeed;
    public float slideAccel;

    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float coyoteTime; //Grace period after falling off a platform, where you can still jump
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime; //Grace period after pressing jump where a jump will be
                                                           //automatically performed once the requirements (being grounded) are met.

    private void OnValidate()
    {
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2)
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale 
        gravityScale = gravityStrength / Physics2D.gravity.y;

        //Calculate the run acceleration and decceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAccelAmount) / runMaxSpeed;
        runDeccelAmount = (50 * runDeccelAmount) / runMaxSpeed;

        //Calculate jumpForce using the formula (initializeJumpVelocity = gravity * timeToJumpToApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        #endregion 
    }


}
