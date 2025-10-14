using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script is in need of revision
/// </summary>

public class PrototypePlayerAttack : MonoBehaviour
{
    [Header("Weapon Setup")]
    public GameObject weaponPrefab; //Variable storing weapon object
    private bool attackDirection;

    PrototypePlayerMovementControls playerController;

    private AudioSource swordSlashAudio;
    /*[SerializeField] */private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PrototypePlayerMovementControls>();
        swordSlashAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        attackDirection = playerController.isFacingRight;
        if(attackDirection )
        {
            weaponPrefab.transform.position = weaponPrefab.transform.position;
        }
        if(!attackDirection )
        {
            weaponPrefab.transform.position = -weaponPrefab.transform.position;
        }
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
            weaponPrefab.SetActive(true);
            animator.SetTrigger("isSlashing");
            swordSlashAudio.Play();
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

    

