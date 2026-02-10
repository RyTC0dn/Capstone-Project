using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    [Space(20)]

    [Header("Audio Clips")]
    public AudioClip[] firstReturnClip;
    public AudioClip noMoneyClip;
    public AudioClip purchaseClip;
    public AudioClip leaveWithoutPayClip;

    public AudioSource devonAudio;
    private AudioSource playerAttackSlash;
    [Space(10)]


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
    public SceneInfo sceneInfo;

    [Header("UI Components")]
    public Button axeButton;
    public Button upgradeButton;
    public GameObject shopFirst;
    public GameObject shopNext;

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
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("AxeBought", 0) == 1)
        {
           axeButton.interactable = false;
        }
        if(PlayerPrefs.GetInt("UpgradeLimit", 0) == 1)
        {
            upgradeButton.interactable = false;
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
            if(isPressed && isNearShop && GameManager.instance.isBlackSmithSaved)
            {
                //Set the shop ui object to active when function is called
                PlayRandomClip(firstReturnClip);

                shopUI.SetActive(true);
                promptButton.SetActive(false);

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

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];

        devonAudio.Stop();
        devonAudio.PlayOneShot(clip);
    }

    public void BuySwordUpgrade()
    {
        //Check if the player has enough coins before buying
        int amount = GameManager.instance.currentCoin;
        int upgradeCap = GameManager.instance.currentUpgrade;
        if(amount >= upgradePrice && upgradeCap < 5)
        {
            devonAudio.PlayOneShot(purchaseClip);
            boughtUpgrade = true;
            GameManager.instance.firstUpgrade = true;
            buyEvent.Raise(this, upgradePrice);            
            upgradeBoughtEvent.Raise(this, boughtUpgrade);
        }
        else
        {
            Debug.Log("Not enough coins");
            devonAudio.PlayOneShot(noMoneyClip);
        }

        if(upgradeCap >= 5)
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
            devonAudio.PlayOneShot(purchaseClip);
            buyEvent.Raise(this, axePrice);
            boughtAxe = true;
            sceneInfo.isAxeBought = true;
            axeBoughtEvent.Raise(this, true);

            EventSystem.current.SetSelectedGameObject(shopNext);

            PlayerPrefs.SetInt("AxeBought", 1);
            PlayerPrefs.Save();
        }
        else
        {
            devonAudio.PlayOneShot(noMoneyClip);
        }
    }

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
            devonAudio.PlayOneShot(leaveWithoutPayClip);
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
            if(isNearShop && GameManager.instance.isBlackSmithSaved) //Also check to see if the npc has been saved
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
}
