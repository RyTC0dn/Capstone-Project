using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public float attackRate = 1f;                        // attacks per second
    private float delayTillAttack;

    [Header("Input / Timing")]
    [Tooltip("Time between pressing the attack button and the attack firing (wind-up).")]
    [SerializeField] private float attackWindup = 0.12f; // delay between input and actual attack
    [Tooltip("How long an input is buffered (queued) while attack is on cooldown).")]
    [SerializeField] private float inputBuffer = 0.18f; // how long an input can be queued
    [SerializeField] private float attackCooldown = 0;
    private bool hasAttacked = false;

    // input buffer state
    private bool attackQueued = false;
    private float bufferTimer = 0f;

    PrototypePlayerMovementControls playerMovement;
    public PlayerCharacter character;

    private AudioSource swordSlashAudio;
    /*[SerializeField] */
    private Animator animator;

    Player_Controller playerControl;

    public ParticleSystem slashVFX;

    private void Awake()
    {
        ////Enable player controller
        //playerControl = PlayerInputHub.controls;

        //playerControl.Gameplay.Melee.performed += OnAttack;
        //playerControl.Gameplay.Melee.canceled += OnAttack;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponentInParent<PrototypePlayerMovementControls>();
        swordSlashAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        weaponCollider.enabled = false;
        delayTillAttack = 1f / attackRate;
    }

    private void Update()
    {
        OnAttack();

        // handle input buffer countdown
        if (attackQueued)
        {
            bufferTimer -= Time.deltaTime;
            if (bufferTimer <= 0f)
            {
                attackQueued = false;
            }
        }

        // if cooldown finished and there is a queued input, fire it
        if (!hasAttacked && attackQueued)
        {
            // consume buffer and start attack immediately (with windup)
            attackQueued = false;
            StartCoroutine(PerformAttackWithWindup());
        }
    }

    /// <summary>
    /// This is the main attack function that is called within Unity on the player input component
    /// If you wish to see the inputs, open the player input Player_Controller, double click Actions of Player Inputs component
    /// our input mapping is "Gameplay", if we want to add new or change certain inputs, open the input map
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack()
    {
        bool key = Mouse.current?.leftButton.wasPressedThisFrame ?? false;
        bool button = Gamepad.current?.rightTrigger.wasPressedThisFrame ?? false;
        bool isPressed = key || button;

        if (isPressed)
        {
            // If we're allowed to attack now, start attack (with optional windup).
            if (!hasAttacked)
            {
                StartCoroutine(PerformAttackWithWindup());
            }
            else
            {
                // otherwise buffer the input so it fires when cooldown finishes
                attackQueued = true;
                bufferTimer = inputBuffer;
            }
        }
        else if (hasAttacked)
        {
            // decrement cooldown until next attack allowed
            delayTillAttack -= Time.deltaTime;
            if (delayTillAttack <= 0f)
            {
                hasAttacked = false;
                delayTillAttack = 1f / attackRate;
            }
        }
    }

    public void DisableAttack()
    {
        playerControl.Gameplay.Melee.Disable();
        swordSlashAudio.enabled = false;
        animator.enabled = false;
        slashVFX.gameObject.SetActive(false);
    }

    public void EnableAttack()
    {
        playerControl.Gameplay.Melee.Enable();
        swordSlashAudio.enabled = true;
        animator.enabled = true;
        slashVFX.gameObject.SetActive(true);
    }

    // Performs the actual attack after windup delay.
    private IEnumerator PerformAttackWithWindup()
    {
        // windup delay before the attack happens
        if (attackWindup > 0f)
            yield return new WaitForSeconds(attackWindup);

        // mark attack started and set cooldown
        hasAttacked = true;
        delayTillAttack = 1f / attackRate;

        // execute character-specific attack behaviour
        switch (character)
        {
            case PlayerCharacter.Knight:
                KnightStandardAttack(); // now only performs visual/sound/collider work
                break;
            case PlayerCharacter.Priest:
                PriestStandardAttack();
                break;
        }
    }

    private void KnightStandardAttack() //This function is to store what the knight does when they attack
    {
        // When function is called, activate collider for weapon collider
        // Trigger the attack animation to start and play audio attached
        // Start coroutine function that turns off collider after animation stops
        weaponCollider.enabled = true;
        animator.SetTrigger("isSlashing");
        swordSlashAudio.Play();

        slashVFX.Play();
        slashVFX.Clear();

        StartCoroutine(ResetWeapon());
    }

    private IEnumerator ResetWeapon() //Reset the collider after animation plays
    {
        yield return new WaitForSeconds(0.6f);
        weaponCollider.enabled = false;
        Debug.Log("Weapon deactivated");
    }

    private void PriestStandardAttack() //This function is to store what the priest does when they attack
    {
        weaponCollider.enabled = true;
        animator.SetTrigger("isSlashing");
        swordSlashAudio.Play();

        StartCoroutine(ResetWeapon());
    }
}

    

