using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Attack_Cleric : MonoBehaviour
{
    /// <summary>
    /// This script will be for activating animation and control for the attack
    /// but the actual damage function will be on PlayerWeapon
    /// </summary>
    #region Attack Variables
    [Header("Weapon Setup")]
    public Collider2D[] weaponColliders; // To track which collider to activate for directional attacks
    public float attackRate = 1f;    // attacks per second                     
    private float delayTillAttack;
    [SerializeField] private GameObject colliderHolder; // empty game object to hold the weapon collider(s) as children
    private bool upHeld, upAttackQueued = false; // Track if the up key is pressed for vertical attacks

    bool isAttacking = false;
    float timeBetweenAttack, timeSinceLastAttack;

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

    private AudioSource swordSlashAudio;
    AudioPlayer audioPlayer;
    private Animator animator;

    Player_Controller playerControl;

    public ParticleSystem slashVFX;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponentInParent<PrototypePlayerMovementControls>();
        swordSlashAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        audioPlayer = GetComponentInChildren<AudioPlayer>();

        for (int i = 0; i < weaponColliders.Length; i++)
        {
            weaponColliders[i].enabled = false;
        }
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
        bool upKey = Keyboard.current?.wKey.isPressed ?? false;
        bool key = Mouse.current?.leftButton.wasPressedThisFrame ?? false;
        bool button = Gamepad.current?.rightTrigger.wasPressedThisFrame ?? false;
        bool isPressed = key || button;

        upHeld = upKey; // Check if up key is pressed for vertical attack

        if (isPressed)
        {
            // If up key is held when attack input is registered, queue vertical attack
            if (upHeld)
            {
                upAttackQueued = true;
            }

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
        else if (upKey)
        {
            colliderHolder.transform.localRotation = Quaternion.Euler(0, 0, 90); // Rotate collider holder for vertical attack
            upAttackQueued = true; // Queue vertical attack if up key is held without pressing attack button
            if (upAttackQueued && isPressed)
            {
                upAttackQueued = false;
                StartCoroutine(PerformAttackWithWindup());
            }
        }
        else
        {
            colliderHolder.transform.localRotation = Quaternion.Euler(0, 0, 0); // Reset collider holder rotation for horizontal attack
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
    private IEnumerator PerformAttackWithWindup(/*Vector2 direction*/)
    {
        // windup delay before the attack happens
        if (attackWindup > 0f)
            yield return new WaitForSeconds(attackWindup);

        // mark attack started and set cooldown
        hasAttacked = true;
        delayTillAttack = 1f / attackRate;

        animator.Play("ClericAttack");
        swordSlashAudio.Play();

        //Add attack audio here for cleric
    }

    private IEnumerator ResetWeapon() //Reset the collider after animation plays
    {
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < weaponColliders.Length; i++)
            weaponColliders[i].enabled = false;
        Debug.Log("Weapon deactivated");
    }

    private void PriestStandardAttack(int collider) //This function is to store what the priest does when they attack
    {
        for (int i = 0; i < weaponColliders.Length; i++)
        {
            weaponColliders[i].enabled = true;
        }
        weaponColliders[collider].enabled = true;

        slashVFX.Play();
        slashVFX.Clear();

        StartCoroutine(ResetWeapon());
    }
}
