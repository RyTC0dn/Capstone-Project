using Unity.AppUI.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WallBreakCharge : MonoBehaviour
{
    private SpriteRenderer characterSP;
    public float chargeTime = 0;
    public float maxCharge;
    private Rigidbody2D rb2D;
    [SerializeField]private bool isCharging = false;
    private bool wallBreakPickedUp = false; //Checks if the wall break upgrade is picked up

    private PrototypePlayerMovementControls playerMove;

    public Image chargeFill;

    public Canvas chargeCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterSP = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PrototypePlayerMovementControls>();
        chargeFill.fillAmount = chargeTime / maxCharge;

        chargeCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(wallBreakPickedUp)
        {
            ChargeMechanic();
        }        
        chargeFill.fillAmount = chargeTime / maxCharge;
    }

    void ChargeMechanic()
    {
        

        bool chargeKey = Keyboard.current.rKey.isPressed;
        bool chargeButton = Gamepad.current?.leftTrigger.isPressed ?? false;
        bool isPressed = chargeKey || chargeButton;

        if (isPressed)
        {
            chargeCanvas.enabled = true; //ReEnable charge fill bar
            chargeTime += Time.deltaTime; //When inputs is held down 
            isCharging = true;
        }
        if(!isPressed && chargeTime > maxCharge)
        {
            chargeCanvas.enabled = false;

            isCharging = false;

            playerMove.enabled = false;

            gameObject.tag = "AbilityPickup";

            Vector2 direction = playerMove.isFacingRight ? Vector2.right : Vector2.left;
            rb2D.linearVelocity = direction * playerMove.playerSpeed * 10f;
            chargeTime = 0;
            Debug.Log("Charge!");

            Invoke(nameof(ReEnableMovement), 0.3f);
        }
        if(!isPressed && chargeTime < maxCharge)
        {
            chargeCanvas.enabled = false;
            chargeTime = 0;
            isCharging = false;
        }
    }

    void ReEnableMovement()
    {
        playerMove.enabled = true;
        gameObject.tag = "Player";
    }

    public void OnWallBreakPickup(Component sender, object data)
    {
        if (data is bool && sender.gameObject == GameObject.Find("WallBreakPickup"))
        {
            bool pickedUp = (bool)data;
            if (pickedUp)
            {
                wallBreakPickedUp = true;
                Debug.Log("Wall Break Ability Ready!");
            }
            else
            {
                Debug.LogError("Wall Break not Ready, check item bool");
            }
        }
    }
}
