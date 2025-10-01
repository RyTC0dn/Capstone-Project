using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypePlayerAttack : MonoBehaviour
{
    [Header("Weapon Setup")]
    //public Transform spawnPosRight; //Storing the position of the spawnpoint of weapon
    //public Transform spawnPosLeft;
    //public GameObject weaponPrefab; //Variable storing weapon object
    private int spawnLimit = 1;
    public int attackValue = 1;
    public int upgradeValue = 0;

    private Vector2 positionRight;
    private Vector2 positionLeft;
    private float positionOffset = 1;

    PrototypePlayerMovementControls playerController;

    [Header("Animation")]
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PrototypePlayerMovementControls>();
        animator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        //Depending on the direction of the player, determines the position of the weapon
        float directionOffset = playerController.isFacingRight ? positionOffset : -positionOffset;
        Vector2 weaponPosition = new Vector2(playerController.transform.position.x + directionOffset, playerController.transform.position.y);

        if(gameObject != null)
        {
            transform.position = weaponPosition;
        }

       

    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        animator.SetBool("AttackRight", playerController.isFacingRight);
        animator.SetBool("AttackLeft", !playerController.isFacingRight);
        //If player is facing right and presses input
        if (context.performed && playerController.isFacingRight)
        {
            animator.enabled = true;
        }

        if(context.performed && !playerController.isFacingRight)
        {
            animator.enabled = true;
        }
    }
}
