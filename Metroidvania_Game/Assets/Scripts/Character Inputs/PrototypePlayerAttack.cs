using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypePlayerAttack : MonoBehaviour
{
    [Header("Weapon Setup")]
    public Transform spawnPosRight; //Storing the position of the spawnpoint of weapon
    public Transform spawnPosLeft;
    public GameObject weaponPrefab; //Variable storing weapon object
    private int spawnLimit = 1;
    public int attackValue = 1;
    public int upgradeValue = 0;

    [SerializeField]
    private float activeTimer = 0.5f;

    private float unsheathTime;
    private bool isUnsheathed = false; //Checks if the player has pressed attack input

    PrototypePlayerMovementControls playerController;

    private GameObject currentWeapon; //Track spawned weapon

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PrototypePlayerMovementControls>();

        unsheathTime = activeTimer; //Make the active timer the default saved by unsheath time        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUnsheathed)
        {
            activeTimer -= Time.deltaTime;
            if (activeTimer <= 0)
            {
                if (currentWeapon != null) { Destroy(currentWeapon, 1); }
                isUnsheathed = false;
                activeTimer = unsheathTime;
            }
        }
    }

    /// <summary>
    /// This is the main attack function that is called within Unity on the player input component
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        //If player is facing left or right and is pressing the left mouse button
        if ((context.performed) && (playerController.isFacingRight || !playerController.isFacingRight))
        {
            //Choose which spawn point based on players direction
            Transform spawnPoint = playerController.isFacingRight ? spawnPosRight : spawnPosLeft;

            //Spawn weapon
            currentWeapon = Instantiate(weaponPrefab, spawnPoint);

            isUnsheathed = true;

            activeTimer = unsheathTime;
        }
    }
}
