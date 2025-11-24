using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrototypeShop : MonoBehaviour
{
    /// <summary>
    /// All UI stuff that pertains to the player UI such as, coin, weapon upgrades, buying axe 
    /// these will be changed for the playtest tomorrow
    /// </summary>

    [Header("General Shop Setup")]
    public GameObject promptButton;
    PrototypePlayerAttack playerAttack;
    public GameObject shopUI;
    private UIManager uiManager;
    public AudioSource devonAudio;
    private AudioSource playerAttackSlash;


    [Header("Game Events")]
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

    public Button axeButton;

    [Header("Prototype End Screen")]
    public GameObject prototypeEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {              
        //Initialize the components
        playerAttack = FindFirstObjectByType<PrototypePlayerAttack>();
        uiManager = FindAnyObjectByType<UIManager>();
        promptButton.SetActive(false);
        devonAudio = gameObject.GetComponent<AudioSource>();
        playerAttackSlash = GameObject.Find("Character 1").GetComponent<AudioSource>();


        //Setting UI components to false on start
        shopUI.SetActive(false);
        prototypeEnd.SetActive(false);
    }

    private void Update()
    {
        if (boughtAxe)
        {
           axeButton.interactable = false;
        }
    }

    //This function is being called by the player movement controls script
    public void EnableShop(Component sender, object data) ///This is for general shopping interactions 
    {
        if (data is bool isPressed)
        {
            if(isPressed && isNearShop && GameManager.instance.isBlacksmithSaved)
            {
                //Set the shop ui object to active when function is called
                devonAudio.Play();

                shopUI.SetActive(true);
                promptButton.SetActive(false);
                playerAttack.enabled = false;
                playerAttackSlash.enabled = false;
                isShopping = true;
                GameManager.instance.StateSwitch(GameStates.Pause);                
            }
        }      
    }

    public void BuySwordUpgrade()
    {
        //Check if the player has enough coins before buying
        int amount = GameManager.instance.currentCoin;
        if(amount >= upgradePrice)
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
        int amount = GameManager.instance.currentCoin;
        if (amount >= axePrice)
        {
            buyEvent.Raise(this, axePrice);
            boughtAxe = true;
            axeBoughtEvent.Raise(this, true);
            PlayerPrefs.SetInt("AxeBought", 1);
            PlayerPrefs.Save();
        }
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        GameManager.instance.StateSwitch(GameStates.Play);
        Invoke(nameof(ReEnablePlayer), 0.3f);
    }

    void ReEnablePlayer()
    {
        playerAttack.enabled = true;
        playerAttackSlash.enabled = true;
    }

    //Check if the player is close to display the interact text 
    //telling the player what button to open the shop
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.CompareTag("Player"))
        {  
            isNearShop = true;
            if(isNearShop && GameManager.instance.isBlacksmithSaved) //Also check to see if the npc has been saved
            {
                promptButton.SetActive(true);
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
                promptButton.SetActive(true);
            }

        }
    }

    //Functions past this point are specifically for the end of the prototype 
    //when players buy something from the shop after saving the blacksmith
    public void ClosePanel()
    {
        prototypeEnd.SetActive(false);
        GameManager.instance.StateSwitch(GameStates.Play);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
