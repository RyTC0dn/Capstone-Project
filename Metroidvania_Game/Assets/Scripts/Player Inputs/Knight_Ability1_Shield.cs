using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Knight_Ability1_Shield : MonoBehaviour
{
    private Animator animator;

    public GameObject shieldCollider;
    public float shieldDuration;
    [SerializeField]private float shieldTimer;
    [SerializeField]private bool shieldEnabled = false; //activates shield when pressing input
    private bool shieldPickedUp = false; //checks if the shield item has been picked up in scene
    PrototypePlayerMovementControls playerMovement;

    public GameObject shieldIcon;

    public SceneInfo sceneInfo;

    private void Start()
    {
        //Initializing script components        
        playerMovement = GetComponent<PrototypePlayerMovementControls>();
        animator = GetComponent<Animator>();        

        //Setting game object components 
        shieldCollider.SetActive(false);
        shieldTimer = shieldDuration;
        shieldIcon.SetActive(false);
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

        if(sceneInfo.isShieldPickedUp)
        {
            //Change idle animation 
            shieldIcon.SetActive(true);
            animator.SetBool("ShieldPickedUp", true);
        }
    }

    public void OnBlockAnimationEnd()
    {
        //Freeze animation on last frame
        animator.speed = 0f;
    }

    public void OnBlock()
    {
        bool playerKey = Keyboard.current.cKey.isPressed;
        bool playerButton = Gamepad.current?.leftTrigger.isPressed ?? false;

        bool isPressed = playerKey || playerButton;
        //If the player holds the shield input = Q key (keyboard) or left trigger (controller)
        if (isPressed && shieldTimer > 0 && shieldPickedUp)
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
        if(data is bool)
        {
            bool pickedUp = sceneInfo.isShieldPickedUp;
            if(pickedUp == true)
            {
                shieldPickedUp = true;
                sceneInfo.isShieldPickedUp = shieldPickedUp;
                Debug.Log("Shield is picked up");
            }
            else
            {
                Debug.LogError("bool is false for picking up shield");
                return;
            }
        }        
    }

    private void EnableShield()
    {
        shieldCollider.SetActive(true);
        shieldEnabled = true;
        playerMovement.horizontalSpeed = playerMovement.playerSpeed / 2;
        animator.Play("Knightblock");
    }

    private void DisableShield()
    {
        shieldCollider.SetActive(false);
        shieldEnabled = false;
        playerMovement.horizontalSpeed = playerMovement.playerSpeed;
        animator.SetBool("isBlocking", false);
        shieldTimer = shieldDuration;
        animator.speed = 1f;
    }
}
