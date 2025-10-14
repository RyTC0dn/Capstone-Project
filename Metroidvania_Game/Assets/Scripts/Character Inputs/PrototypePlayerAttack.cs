using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script is in need of revision
/// </summary>

public class PrototypePlayerAttack : MonoBehaviour
{
    [Header("Weapon Setup")]
    public Transform spawnPosRight; //Storing the position of the spawnpoint of weapon
    public Transform spawnPosLeft;
    public GameObject weaponPrefab; //Variable storing weapon object
    private bool attackDirection;

    [SerializeField]
    private float activeTimer = 0.5f;

    private float unsheathTime;
    private bool isUnsheathed = false; //Checks if the player has pressed attack input

    PrototypePlayerMovementControls playerController;

    private AudioSource swordSlashAudio;
    /*[SerializeField] */private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PrototypePlayerMovementControls>();

        unsheathTime = activeTimer; //Make the active timer the default saved by unsheath time        
        swordSlashAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// This is the main attack function that is called within Unity on the player input component
    /// </summary>
    /// <param name="context"></param>
    /// 
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            animator.SetTrigger("isSlashing");
        }
    }

    //public void OnAttack(InputAction.CallbackContext context)
    //{
    //    //If player is facing left or right, pressing input and the weapon hasn't been instantiated yet
    //    if (context.performed && (playerController.isFacingRight || !playerController.isFacingRight) && !isUnsheathed)
    //    {
    //        //Choose which spawn point based on players direction
    //        Transform spawnPoint = playerController.isFacingRight ? spawnPosRight : spawnPosLeft;


    //        //animator.SetBool("isSlashing", true); //switch to slashing animation

    //    }
    //    //Play audio file for sword slash
    //    swordSlashAudio.Play();

    //    isUnsheathed = true;

    //    activeTimer = unsheathTime;

    //    }

}

    

