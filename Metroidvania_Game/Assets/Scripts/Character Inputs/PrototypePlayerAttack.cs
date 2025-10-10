using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypePlayerAttack : MonoBehaviour
{
    [Header("Weapon Setup")]
    public Transform spawnPosRight; //Storing the position of the spawnpoint of weapon
    public Transform spawnPosLeft;
    public GameObject weaponPrefab; //Variable storing weapon object

    [SerializeField]
    private float activeTimer = 0.5f;

    private float unsheathTime;
    private bool isUnsheathed = false; //Checks if the player has pressed attack input

    PrototypePlayerMovementControls playerController;

    private GameObject currentWeapon; //Track spawned weapon
    private AudioSource swordSlashAudio;
    [SerializeField] private Animator animator;
    private bool isAnimatorOn;

    public int animationTimer = 10;
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
        if (isUnsheathed)
        {
            StartCoroutine(DestroyWeapon());
           
        }
    }

    /// <summary>
    /// This  function is not only to ensure that there is a timer that gets rid of the weapon prefab
    /// but also ensure that no more than one can be spawned at a time to avoid any bugs
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyWeapon() 
    {
        yield return new WaitForSeconds(0.5f);
        isUnsheathed = false;
        Destroy(currentWeapon);
    }

    /// <summary>
    /// This is the main attack function that is called within Unity on the player input component
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetBool("isSlashing", true); //switch to slashing animation
            isAnimatorOn = true;
            
        }
        if (isAnimatorOn)
        {
            animationTimer--;
        }

        else if(animationTimer <= 0)
        {
            animator.SetBool("isSlashing", false); //switch to idle animation
        }
        ////If player is facing left or right, pressing input and the weapon hasn't been instantiated yet
        //if (context.performed && (playerController.isFacingRight || !playerController.isFacingRight) && !isUnsheathed)
        //{
        //    //Choose which spawn point based on players direction
        //    Transform spawnPoint = playerController.isFacingRight ? spawnPosRight : spawnPosLeft;

        //    //Spawn weapon
        //    currentWeapon = Instantiate(weaponPrefab, spawnPoint);


        //    //Weapon Sprite 
        //    if (playerController.isFacingRight)
        //    {
        //        weaponPrefab.GetComponent<SpriteRenderer>().flipX = false;
        //    }
        //    else
        //    {
        //        weaponPrefab.GetComponent<SpriteRenderer>().flipX = true;
        //    }

        //    //animator.SetBool("isSlashing", true); //switch to slashing animation

        //}
        //Play audio file for sword slash
        swordSlashAudio.Play();

        isUnsheathed = true;

        activeTimer = unsheathTime;

        //if (unsheathTime == 0)
         //   {          
         //       animator.SetBool("isSlashing", false); //if the unsheath time is out switch animations
         //   }
        }

    }

    

