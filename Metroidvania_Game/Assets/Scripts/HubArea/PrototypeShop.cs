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
    PrototypePlayerAttack playerAttack;
    public GameObject shopUI;
    private UIManager uiManager;

    public GameEvent buyEvent; //For whenever the player buys something
    public GameEvent axeBoughtEvent; //
    public GameEvent upgradeBoughtEvent; //Checking if upgrade bought to update UI

    [Header("Shop Prices")]
    ///Shop prices
    ///May turn to using arrays to store prices as to not clutter too much
    public int upgradePrice = 8;
    public int axePrice = 2;

    public bool isNearShop = false;
    private bool isShopping = false;

    [SerializeField]private bool boughtAxe = false;
    [SerializeField]private bool boughtUpgrade = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {              
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
        //Check if the player has enough coins before buying
        PlayerUI playerUI = FindFirstObjectByType<PlayerUI>();
        if(playerUI != null && playerUI.currentCoin >= upgradePrice)
        {
            buyEvent.Raise(this, upgradePrice);
            boughtUpgrade = true;
            upgradeBoughtEvent.Raise(this, boughtUpgrade);
        }
        else
        {
            Debug.Log("Not enough coins");
            //Can have an audio play here
        }
      
    }

    public void BuyAxe() //Function for buying the axe
    {
        PlayerUI playerUI = FindFirstObjectByType<PlayerUI>();
        if (playerUI != null && playerUI.currentCoin >= axePrice)
        {
            buyEvent.Raise(this, axePrice);
            boughtAxe = true;
            axeBoughtEvent.Raise(this, true);
        }
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
