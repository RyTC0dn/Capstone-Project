using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Knight_Ability1_Shield : MonoBehaviour
{
    private Animator animator;

    public GameObject shieldCollider;
    public float shieldDuration;
    [SerializeField]private float shieldTimer;
    [SerializeField] private bool shieldEnabled; //activates shield when pressing input
    private bool shieldSelected = false; //checks if the shield item has been picked up in scene
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

        if (sceneInfo.isShieldUsed && !sceneInfo.isAxeUsed)
        {
            shieldSelected = true;            
        }
    }

    private void Update()
    {
        if(sceneInfo == null)
        {
            Debug.LogError("SceneInfo reference is missing in Knight_Ability1_Shield script.");
            return;
        }

        if (sceneInfo.isShieldPickedUp)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                DisableShield();
            }
        }

        OnBlock();

        if(shieldSelected)
        {
            //Change idle animation 
            shieldIcon.SetActive(true);
            animator.SetBool("ShieldPickedUp", true);
        }
        else
        {
            //Change idle animation 
            shieldIcon.SetActive(false);
            animator.SetBool("ShieldPickedUp", false);
        }
    }

    public void OnBlockAnimationEnd()
    {
        //Freeze animation on last frame
        animator.speed = 0f;
    }

    public void OnBlock()
    {
        bool playerKey = Keyboard.current.qKey.isPressed;
        bool playerButton = Gamepad.current?.leftTrigger.isPressed ?? false;

        bool isPressed = playerKey || playerButton;
        //If the player holds the shield input = Q key (keyboard) or left trigger (controller)
        if (isPressed && shieldTimer > 0 && shieldSelected)
        {
            EnableShield();
        }
        else
        {
            DisableShield();
        }
    }
    #region Menu Button Logic
    //Called during button press
    public void OnButtonSelected()
    {
        shieldSelected = true;
        sceneInfo.isShieldUsed = true; //Set shield as used when selected in menu
    }

    public void OnButtonDeselect()
    {
        shieldSelected = false;
        sceneInfo.isShieldUsed = false; //Set shield as not used when deselected in menu
    }
    #endregion

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
