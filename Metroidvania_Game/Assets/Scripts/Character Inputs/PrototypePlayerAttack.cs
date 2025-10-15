using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script is in need of revision
/// </summary>
/// 

public enum PlayerCharacter
{
    Knight, 
    Priest
}

public class PrototypePlayerAttack : MonoBehaviour
{
    [Header("Weapon Setup")]
    public GameObject weaponPrefab; //Variable storing weapon object
    public int damageValue = 1;

    PrototypePlayerMovementControls playerController;
    public PlayerCharacter character;

    private AudioSource swordSlashAudio;
    /*[SerializeField] */private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PrototypePlayerMovementControls>();
        swordSlashAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        weaponPrefab = GetComponentInChildren<GameObject>();
        weaponPrefab.SetActive(false);
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
            switch (character)
            {
                case PlayerCharacter.Knight:
                    KnightStandardAttack();
                    break;
                case PlayerCharacter.Priest:
                    PriestStandardAttack();
                    break;
            }           
        }
    }

    private void KnightStandardAttack()//This function is to store what the knight does when they attack
    {
        
        animator.SetTrigger("isSlashing");
        swordSlashAudio.Play();
    }

    private void PriestStandardAttack()//This function is to store what the priest does when they attack
    {

    }
}

    

