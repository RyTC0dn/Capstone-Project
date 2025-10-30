using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypeShop : MonoBehaviour
{
    /// <summary>
    /// All UI stuff that pertains to the player UI such as, coin, weapon upgrades, buying axe 
    /// these will be changed for the playtest tomorrow
    /// </summary>

    [Header("General Shop Setup")]
    public TextMeshProUGUI interactText;
    PrototypePlayerMovementControls playerMovementControls;
    PrototypePlayerAttack playerAttack;
    public GameObject shopUI;
    private UIManager uiManager;
    public bool boughtAxe = false;

    public GameEvent buyEvent; //For whenever the player buys something

    [Header("Shop Prices")]
    ///Shop prices
    ///May turn to using arrays to store prices as to not clutter too much
    public int upgradePrice = 8;
    public int weaponUpgradeValue = 1;
    public int axePrice = 2;

    public bool isNearShop = false;
    private bool isShopping = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {              
        playerMovementControls = FindFirstObjectByType<PrototypePlayerMovementControls>();
        playerAttack = FindFirstObjectByType<PrototypePlayerAttack>();
        uiManager = FindAnyObjectByType<UIManager>();
        interactText.enabled = false;

        shopUI = GetComponentInChildren<GameObject>();
        shopUI.SetActive(isShopping);
    }

    private void Update()
    {

    }

    //This function is being called by the player movement controls script
    public void EnableShop(Component sender, object data) ///This is for general shopping interactions 
    {
        if (data is bool isPressed)
        {
            if(isPressed && isNearShop && GameManager.instance.isNPCSaved)
            {
                //Set the shop ui object to active when function is called
                shopUI.SetActive(true);
                interactText.enabled = false;
                playerAttack.enabled = false;
                GameManager.instance.StateSwitch(GameStates.Pause);
            }
        }      
    }

    public void BuySwordUpgrade()
    {
        buyEvent.Raise(this, upgradePrice);        
    }

    public void BuyAxe() //Function for buying the axe
    {
        buyEvent.Raise(this, axePrice);
    }

    public void Display(string hoverText)
    {
        interactText.text = hoverText;
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        GameManager.instance.StateSwitch(GameStates.Play);

    }

    //Check if the player is close to display the interact text 
    //telling the player what button to open the shop
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.CompareTag("Player"))
        {  
            isNearShop = true;
            if(isNearShop && GameManager.instance.isNPCSaved) //Also check to see if the npc has been saved
            {
                interactText.enabled = true;
                string text = "Press E to Interact";
                Display(text);
            }
 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            isNearShop = false;
            if (!isNearShop)
            {
                interactText.enabled = false;
            }

        }
    }
}
