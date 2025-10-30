using UnityEngine;
using UnityEngine.InputSystem;
public class PrototypeShield : MonoBehaviour
{
    private Animator animator;

    public GameObject shieldCollider;
    public float shieldDuration;
    [SerializeField]private float shieldTimer;
    private bool shieldEnabled = false;
    PrototypePlayerMovementControls playerMovement;

    private void Start()
    {
        shieldCollider.SetActive(false);
        playerMovement = GetComponent<PrototypePlayerMovementControls>();
        animator = GetComponent<Animator>();
        shieldTimer = shieldDuration;
    }

    private void Update()
    {
        if (shieldEnabled)
        {
            shieldTimer -= Time.deltaTime;
        }
        else if(shieldTimer <= 0) 
        {
            shieldEnabled = false;
            shieldTimer = shieldDuration;
        }
    }

    public void OnShield(InputAction.CallbackContext context)
    {
        //If the player holds the shield input = Q key (keyboard) or left trigger (controller)
        if (context.performed && shieldTimer > 0)
        {
            shieldCollider.SetActive(true);
            shieldEnabled = true;
            playerMovement.horizontalSpeed = playerMovement.playerSpeed / 2;
        }
        else
        {
            shieldCollider.SetActive(false);
            playerMovement.horizontalSpeed = playerMovement.playerSpeed;
        }
    }
}
