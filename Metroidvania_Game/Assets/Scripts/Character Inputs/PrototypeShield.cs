using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PrototypeShield : MonoBehaviour
{
    private Animator animator;
    private Slider shieldBar;

    public GameObject shieldCollider;
    public float shieldDuration;
    [SerializeField]private float shieldTimer;
    [SerializeField]private bool shieldEnabled = false; //activates shield when pressing input
    private bool shieldPickedUp = false; //checks if the shield item has been picked up in scene
    PrototypePlayerMovementControls playerMovement;

    private void Start()
    {
        //Initializing script components        
        playerMovement = GetComponent<PrototypePlayerMovementControls>();
        animator = GetComponent<Animator>();        
        shieldBar = GetComponentInChildren<Slider>();

        //Setting game object components 
        shieldCollider.SetActive(false);
        shieldTimer = shieldDuration;
    }

    private void Update()
    {
        if (shieldEnabled)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                DisableShield();
            }
        }

        OnBlock();
    }

    public void OnBlockAnimationEnd()
    {
        //Freeze animation on last frame
        animator.speed = 0f;
    }

    public void OnBlock()
    {
        bool playerKey = Keyboard.current.qKey.isPressed;
        bool playerButton = Gamepad.current.leftTrigger.isPressed;
        //If the player holds the shield input = Q key (keyboard) or left trigger (controller)
        if ((playerKey || playerButton) && shieldTimer > 0)
        {
            EnableShield();
        }
        else
        {
            DisableShield();
        }
    }

    public void OnShieldPickUp(Component sender, object data)
    {
        if(data is bool pickedUp)
        {
            if(pickedUp == true)
            {
                shieldPickedUp = true;
            }           
        }        
    }

    private void EnableShield()
    {
        shieldCollider.SetActive(true);
        shieldEnabled = true;
        playerMovement.horizontalSpeed = playerMovement.playerSpeed / 2;
        animator.SetBool("isBlocking", true);
    }

    private void DisableShield()
    {
        shieldCollider.SetActive(false);
        shieldEnabled = false;
        playerMovement.horizontalSpeed = playerMovement.playerSpeed;
        animator.SetBool("isBlocking", false);
        shieldTimer = shieldDuration;        
    }
}
