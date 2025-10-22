using UnityEngine;
using UnityEngine.InputSystem;
public class PrototypeShield : MonoBehaviour
{
    public GameObject shieldCollider;
    public float shieldDuration;
    private bool shieldEnabled = false;
    PrototypePlayerMovementControls playerMovement;

    private void Start()
    {
        shieldCollider.SetActive(false);
        playerMovement = GetComponent<PrototypePlayerMovementControls>();
    }

    private void Update()
    {
        if (shieldEnabled)
        {
            shieldDuration -= Time.deltaTime;
        }
    }

    public void OnShield(InputAction.CallbackContext context)
    {
        //If the player holds the shield input = Q key (keyboard) or left trigger (controller)
        if (context.performed && shieldDuration > 0)
        {
            shieldCollider.SetActive(true);
            shieldEnabled = true;
            playerMovement.horizontalSpeed = playerMovement.playerSpeed / 2;
        }
        else if (shieldDuration <= 0)
        {
            shieldCollider.SetActive(false);
            playerMovement.horizontalSpeed = playerMovement.playerSpeed;
        }
    }
}
