using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerCharacter
{
    Knight, 
    Priest
}

public class PrototypePlayerAttack : MonoBehaviour
{
    /// <summary>
    /// This script will be for activating animation and control for the attack 
    /// but the actual damage function will be on PlayerWeapon
    /// </summary>

    [Header("Weapon Setup")]
    public Collider2D weaponCollider;

    PrototypePlayerMovementControls playerController;
    public PlayerCharacter character;

    private AudioSource swordSlashAudio;
    /*[SerializeField] */private Animator animator;

    Player_Controller playerControl;

    public ParticleSystem slashVFX;

    private void Awake()
    {
        playerControl = new Player_Controller();
        playerControl.Enable();

        playerControl.Gameplay.Melee.performed += OnAttack;
        playerControl.Gameplay.Melee.canceled += OnAttack;
    }

    private void OnDestroy()
    {
        playerControl.Gameplay.Melee.performed -= OnAttack;
        playerControl.Gameplay.Melee.canceled -= OnAttack;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PrototypePlayerMovementControls>();
        swordSlashAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        weaponCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// This is the main attack function that is called within Unity on the player input component
    /// If you wish to see the inputs, open the player input Player_Controller, double click Actions of Player Inputs component
    /// our input mapping is "Gameplay", if we want to add new or change certain inputs, open the input map 
    /// </summary>
    /// <param name="context"></param>
    /// 
    public void OnAttack(InputAction.CallbackContext context)
    {
        ///Attack function is being managed by 

        if(context.performed)
        {
            StartCoroutine(Delay());
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
        //When function is called, activate collider for weapon collider
        //Trigger the attack animation to start and play audio attached
        //Start coroutine function that turns off collider after animation stops
        weaponCollider.enabled = true;
        animator.SetTrigger("isSlashing");
        swordSlashAudio.Play();

        StartCoroutine(ResetWeapon());
    }

    private IEnumerator ResetWeapon()
    {
        slashVFX.Play();

        yield return new WaitForSeconds(0.6f);
        weaponCollider.enabled = false;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.6f);
    }

    private void PriestStandardAttack()//This function is to store what the priest does when they attack
    {
        weaponCollider.enabled = true;
        animator.SetTrigger("isSlashing");
        swordSlashAudio.Play();

        StartCoroutine(ResetWeapon());
    }
}

    

