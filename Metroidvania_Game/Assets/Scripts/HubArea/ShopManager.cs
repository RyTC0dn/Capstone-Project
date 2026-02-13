using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    /// <summary>
    /// All UI stuff that pertains to the player UI such as, coin, weapon upgrades, buying axe 
    /// these will be changed for the playtest tomorrow
    /// </summary>

    [Header("General Shop Setup")]
    PrototypePlayerAttack playerAttack;
    public GameObject shopUI;
    private bool firstOccurence;
    private UIManager uiManager;
    [Space(20)]

    [Header("Audio Clips")]
    [SerializeField]private int firstOccurrenceElement;
    [SerializeField] private int regularOccurrenceElement;
    [SerializeField]private int noMoneyElement;
    [SerializeField] private int purchaseElement;
    [SerializeField] private int leaveWithoutPayElement;
    private AudioPlayer player;
    private bool firstVisit = false;
    [Tooltip("Toggle on if not using regular occurence element")]
    [SerializeField]private bool notUsed = false;

    public AudioSource audioSource;
    private AudioSource playerAttackSlash;
    [Space(10)]


    [Header("Game Events")]
    public GameEvent buyEvent; //For whenever the player buys something
    public GameEvent secondItemEvent; //
    public GameEvent firstItemEvent; //Checking if upgrade bought to update UI

    [Header("Shop Prices")]
    ///Shop prices
    ///May turn to using arrays to store prices as to not clutter too much
    public int upgradePrice;
    public int axePrice;
    public int healthPrice;
    public int strengthPrice;

    public bool isNearShop = false;
    private bool isShopping = false;
    [SerializeField]private bool boughtAxe = false;
    [SerializeField]private bool boughtUpgrade = false;
    [SerializeField] private bool boughtHealth = false;
    public SceneInfo sceneInfo;

    [Header("UI Components")]
    public Button secondItemButton;
    public Button firstItemButton;
    public GameObject shopFirst;
    public GameObject shopNext;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {              
        //Initialize the components
        playerAttack = FindFirstObjectByType<PrototypePlayerAttack>();
        uiManager = FindAnyObjectByType<UIManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
        playerAttackSlash = GameObject.Find("Character 1").GetComponent<AudioSource>();
        player = GetComponent<AudioPlayer>(); 


        //Setting UI components to false on start
        shopUI.SetActive(false);
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("AxeBought", 0) == 1)
        {
           secondItemButton.interactable = false;
        }
        else if(PlayerPrefs.GetInt("UpgradeLimit", 0) == 1)
        {
            firstItemButton.interactable = false;
        }

        //Close shop shortcut
        bool keyInput = Keyboard.current.escapeKey.isPressed;
        bool buttonInput = Gamepad.current?.bButton.isPressed ?? false;

    }

    //This function is being called by the player movement controls script
    public void EnableShop(Component sender, object data) ///This is for general shopping interactions 
    {
        if (data is bool isPressed)
        {
            if (isPressed && isNearShop && (GameManager.instance.isBlackSmithSaved || GameManager.instance.isPotionMakerSaved))
            {
                //Set the shop ui object to active when function is called
                if (!firstOccurence)
                    player.PlayAudio(firstOccurrenceElement, audioSource);
                else
                {
                    if (!notUsed)
                    {
                        player.PlayAudio(regularOccurrenceElement, audioSource);
                        firstOccurence = true;
                    }
                }
                    

                shopUI.SetActive(true);

                //Enable event system once
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(shopFirst);

                playerAttack.enabled = false;
                playerAttackSlash.enabled = false;
                isShopping = true;
                GameManager.instance.StateSwitch(GameStates.Pause);
            }
        }      
    }

    /// <summary>
    /// This is where the purchase logic for each item for the Blacksmith will stay
    /// </summary>
    #region Blacksmith Items
    public void BuySwordUpgrade() //Buy sword strength upgrade function
    {
        //Check if the player has enough coins before buying
        int amount = GameManager.instance.currentCoin;
        int upgradeCap = GameManager.instance.currentUpgrade;
        if(amount >= upgradePrice && upgradeCap < 5)
        {
            player.PlayAudio(purchaseElement, audioSource);
            boughtUpgrade = true;
            GameManager.instance.firstUpgrade = true;
            buyEvent.Raise(this, upgradePrice);            
            firstItemEvent.Raise(this, boughtUpgrade);
        }
        else
        {
            Debug.Log("Not enough coins");
            player.PlayAudio(noMoneyElement, audioSource);
        }

        if(upgradeCap >= 5) //Limit 
        {
            PlayerPrefs.SetInt("UpgradeLimit", 1);
            PlayerPrefs.Save();
        }
      
    }

    public void BuyAxe() //Function for buying the axe
    {
        int amount = GameManager.instance.currentCoin;
        if (amount >= axePrice)
        {
            //If purchased, play audio cue
            player.PlayAudio(purchaseElement, audioSource);
            buyEvent.Raise(this, axePrice);
            boughtAxe = true;
            sceneInfo.isAxeBought = true;
            secondItemEvent.Raise(this, true);

            EventSystem.current.SetSelectedGameObject(shopNext);

            PlayerPrefs.SetInt("AxeBought", 1);
            PlayerPrefs.Save();
        }
        else
        {
            player.PlayAudio(noMoneyElement, audioSource);
        }
    }
    #endregion

    /// <summary>
    /// This is where the purchase logic for each item in the Potion shop are held
    /// </summary>
    #region Alchemist Items
    public void BuyHealthPotion()
    {
        int amount = GameManager.instance.currentCoin;
        if(amount >= healthPrice)
        {
            player.PlayAudio(purchaseElement, audioSource);
            buyEvent.Raise(this, healthPrice);

            sceneInfo.isHPBought = true;
        }
        else if(amount <= 0)
        {
            player.PlayAudio(noMoneyElement, audioSource);
        }
    }
    #endregion

    public void CloseShop()
    {
        //Disable event system
        EventSystem.current.SetSelectedGameObject(null);

        shopUI.SetActive(false); 

        GameManager.instance.StateSwitch(GameStates.Play);
        Invoke(nameof(ReEnablePlayer), 0.3f);
        if(!boughtAxe && !boughtUpgrade)
        {
            //Play audio clip if player hasn't bought anything from shop
            player.PlayAudio(leaveWithoutPayElement, audioSource);
        }
        else if (!boughtHealth)
        {
            //Play audio clip if player hasn't bought anything from shop
            player.PlayAudio(leaveWithoutPayElement, audioSource);
        }
        else
        {
            return;
        }
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            isNearShop = false;
        }
    }
}
