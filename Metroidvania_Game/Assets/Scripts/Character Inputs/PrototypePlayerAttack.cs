using UnityEngine;

public class PrototypePlayerAttack : MonoBehaviour
{
    [Header("Weapon Setup")]
    public Transform spawnPosRight; //Storing the position of the spawnpoint of weapon
    public Transform spawnPosLeft;
    public GameObject weaponPrefab; //Variable storing weapon object
    private int currentSpawn = 0;

    [SerializeField]
    private float activeTimer = 0.5f;

    private float unsheathTime;
    private bool isUnsheathed = false; //Checks if the player has pressed attack input

    PlayerMovementControls playerController;

    private GameObject currentWeapon; //Track spawned weapon

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PlayerMovementControls>();

        unsheathTime = activeTimer; //Make the active timer the default saved by unsheath time
    }

    // Update is called once per frame
    void Update()
    {
        WeaponUnsheath();        
    }

    private void WeaponUnsheath()
    {
        if (Input.GetMouseButtonDown(0) && (playerController.isFacingRight || !playerController.isFacingRight))
        {
            //Choose which spawn point based on players direction
            Transform spawnPoint = playerController.isFacingRight ? spawnPosRight : spawnPosLeft;

            //if (playerController.isFacingRight) { Debug.Log("Facing right"); }
            //else if(!playerController.isFacingRight) { Debug.Log("Facing Left"); }
            //if(spawnPoint == false) { Debug.LogWarning("Weapon not able to spawn left, check code!"); }

            //Spawn weapon
            currentWeapon = Instantiate(weaponPrefab, spawnPoint);

            isUnsheathed=true;

            activeTimer = unsheathTime;
        }
        else if (isUnsheathed)
        {
            activeTimer -= Time.deltaTime;
            if(activeTimer <= 0)
            {
                if (currentWeapon != null) { Destroy(currentWeapon); }
                isUnsheathed = false;
                activeTimer = unsheathTime;
            }
        }
        //if (Input.GetMouseButton(0) && playerController.isFacingRight)
        //{
        //    Instantiate(weapon, );
        //    isUnsheathed = true;

        //    //Reset time every time players hit the input
        //    activeTimer = unsheathTime;
        //}
        //else if(isUnsheathed)
        //{
        //    activeTimer -= Time.deltaTime;
        //    if (activeTimer <= 0)
        //    {
        //        Destroy(weapon);
        //        isUnsheathed = false;

        //        //Extra reset precautions so the timer is ready for next input
        //        activeTimer = unsheathTime;
        //    }
        //}


    }
}
