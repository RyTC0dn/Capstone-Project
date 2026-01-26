using System.Collections;
using System.Timers;
using Unity.AppUI.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WallBreakCharge : MonoBehaviour
{
    [Header("References")]
    private Animator animator;
    private SpriteRenderer characterSP;
    public float chargeTime = 0;
    public float maxCharge;
    private Rigidbody2D rb2D;
    [SerializeField]private bool isCharging = false;
    [SerializeField]private bool wallBreakPickedUp = false; //Checks if the wall break upgrade is picked up

    private PrototypePlayerMovementControls playerMove;

    [SerializeField]private float chargeDistance;
    [SerializeField] private float chargeMultiplier;
    public SceneInfo sceneInfo;

    public Image chargeFill;

    public Canvas chargeCanvas;

    public GameObject dashTrail;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        #region References
        characterSP = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PrototypePlayerMovementControls>();
        animator = GetComponent<Animator>();
        chargeFill.fillAmount = chargeTime / maxCharge;
        #endregion

        chargeCanvas.enabled = false;
        dashTrail.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(sceneInfo.isWallBreakPickedUp)
        {
            ChargeMechanic();
        }
        chargeFill.fillAmount = chargeTime / maxCharge;
    }

    void ChargeMechanic()
    {       

        bool chargeKey = Keyboard.current.rKey.isPressed;
        bool chargeButton = Gamepad.current?.leftShoulder.isPressed ?? false;
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

            dashTrail.SetActive(true);

            Vector2 direction = playerMove.isFacingRight ? Vector2.right : Vector2.left;
            animator.Play("KnightCharge");
            StartCoroutine(ChargeDash(direction));
            //rb2D.linearVelocity = direction * chargeDistance * chargeMultiplier;
            chargeTime = 0;
            Debug.Log("Charge!");

            StartCoroutine(DisableDash());
        }
        if(!isPressed && chargeTime < maxCharge)
        {
            chargeCanvas.enabled = false;
            chargeTime = 0;
            isCharging = false;
        }
    }

    private IEnumerator DisableDash()
    {
        yield return new WaitForSeconds(0.6f);
        dashTrail.SetActive(false);
    }

    private IEnumerator ChargeDash(Vector2 direction)
    {
        float dashDuration = 0.3f;
        float elapsed = 0f;

        //Disable gravity + drag so it feels clean
        float originalGravity = rb2D.gravityScale;
        rb2D.gravityScale = 0;

        //Freeze movement
        playerMove.enabled = false;

        while (elapsed < dashDuration)
        {
            elapsed += Time.deltaTime;

            //Lerp velocity for smooth acceleration
            rb2D.linearVelocity = direction * Mathf.Lerp(0, chargeDistance * chargeMultiplier, elapsed/dashDuration);

            yield return null;
        }

        rb2D.linearVelocity = Vector2.zero;
        rb2D.gravityScale = originalGravity;

        //Restore control
        playerMove.enabled = true;
        gameObject.tag = "Player";
    }

    public void OnWallBreakPickup(Component sender, object data)
    {
        if (data is bool && sender.gameObject.name == "WallBreakPickup")
        {
            bool pickedUp = sceneInfo.isWallBreakPickedUp;
            if (pickedUp)
            {
                wallBreakPickedUp = true;
                sceneInfo.isWallBreakPickedUp = wallBreakPickedUp;
                Debug.Log("Wall Break Ability Ready!");
            }
            else
            {
                Debug.LogError("Wall Break not Ready, check item bool");
            }
        }
    }
}
