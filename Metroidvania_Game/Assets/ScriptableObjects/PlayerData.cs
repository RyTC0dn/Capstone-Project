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
    [Range(0, 1)] public float jumpHangeGravityMult; //

}
