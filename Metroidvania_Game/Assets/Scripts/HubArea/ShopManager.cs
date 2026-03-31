using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ShopType
{
    None,
    Blacksmith,
    Alchemist,
    Priest
}

/// <summary>
/// Manages shop interactions, item purchases, and related UI and game events within the game scene.
/// </summary>
/// <remarks>The ShopManager coordinates player access to shops, handles purchase logic for various items, and
/// updates UI elements and game state accordingly. It supports multiple shop types, such as Blacksmith and Alchemist,
/// and raises events when items are bought to allow other systems to respond. ShopManager should be attached to a shop
/// GameObject in the scene and requires references to relevant UI components and game events. Shop access and purchase
/// actions are enabled based on player proximity and game progress. This class is not thread-safe and is intended for
/// use within Unity's main thread.</remarks>
public class ShopManager : MonoBehaviour
{
    [Header("General Shop Setup")]
    private Player_Knight_Attack playerAttack;

    public GameObject shopUI;
    private bool firstOccurence;
    private UIManager uiManager;
    public ShopType shopType;

    [Space(20)]
    [Header("Audio Clips")]
    [SerializeField] private int firstOccurrenceElement;

    [SerializeField] private int regularOccurrenceElement;
    [SerializeField] private int noMoneyElement;
    [SerializeField] private int purchaseElement;
    [SerializeField] private int leaveWithoutPayElement;
    private AudioPlayer player;
    private bool firstVisit = false;

    [Tooltip("Toggle on if not using regular occurence element")]
    [SerializeField] private bool notUsed = false;

    public AudioSource audioSource;
    private AudioSource playerAttackSlash;

    [Space(10)]
    [Header("Game Events")]
    public GameEvent secondItemEvent; //

    public GameEvent firstItemEvent; //Checking if upgrade bought to update UI

    [Header("Shop Prices")]
    [Tooltip("Set price only if enum state is set for Blacksmith")]
    public int swordUpgradePrice;

    public int knockbackUpgradePrice;

    public int axePrice;
    public int healthPrice;
    public int strengthPrice;

    public bool isNearShop = false;
    private bool isShopping = false;
    [SerializeField] private bool boughtAxe = false;
    [SerializeField] private bool boughtUpgrade = false;
    [SerializeField] private bool boughtKnockback = false;
    [SerializeField] private bool boughtHealth = false;
    public SceneInfo sceneInfo;
    private bool isSaved = false;

    [Header("UI Components")]
    public Button secondItemButton;

    public Button firstItemButton;
    public GameObject shopFirst;
    public GameObject shopNext;

    [Tooltip("Assign Axe price text, health potion text")]
    public TextMeshProUGUI firstItemText;

    [Tooltip("Assign Weapon Upgrade, Strength potion text")]
    public TextMeshProUGUI secondItemText;

    [Tooltip("Assign Knockback text")]
    public TextMeshProUGUI thirdItemText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //Initialize the components
        playerAttack = FindFirstObjectByType<Player_Knight_Attack>();
        uiManager = FindAnyObjectByType<UIManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
        playerAttackSlash = GameObject.Find("Character 1").GetComponent<AudioSource>();
        player = GetComponent<AudioPlayer>();

        //Setting UI components to false on start
        shopUI.SetActive(false);

        //Initial price set
        UpdatePrice();
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("AxeBought", 0) == 1)
        {
            secondItemButton.interactable = false;
        }
        else if (PlayerPrefs.GetInt("UpgradeLimit", 0) == 1)
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
            switch (shopType)
            {
                case ShopType.None:
                    break;

                case ShopType.Blacksmith:
                    isSaved = GameManager.instance.isBlackSmithSaved;
                    sceneInfo.talkedToBlacksmith = true;
                    break;

                case ShopType.Alchemist:
                    isSaved = GameManager.instance.isPotionMakerSaved;
                    sceneInfo.talkedToAlchemist = true;
                    break;

                case ShopType.Priest:
                    isSaved = GameManager.instance.isHealerSaved;
                    sceneInfo.talkedToPriest = true;
                    break;

                default:
                    break;
            }
            if (isPressed && isNearShop && isSaved)
            {
                //Set the shop ui object to active when function is called
                if (!firstOccurence)
                    player.PlayAudio(firstOccurrenceElement, audioSource, false);
                else
                {
                    if (!notUsed)
                    {
                        player.PlayAudio(regularOccurrenceElement, audioSource, false);
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

    private void UpdatePrice()
    {
        #region Price Change Text

        switch (shopType) //Determine price items based on enum
        {
            case ShopType.None:
                break;

            case ShopType.Blacksmith:
                firstItemText.text = $"Throwable Axe: " + axePrice.ToString();
                secondItemText.text = $"Sword Upgrade: " + swordUpgradePrice.ToString();
                thirdItemText.text = "Knockback Upgrade: " + knockbackUpgradePrice.ToString();
                break;

            case ShopType.Alchemist:
                firstItemText.text = $"Health Potion: " + healthPrice.ToString();
                secondItemText.text = $"Strength Potion: " + strengthPrice.ToString();
                break;

            case ShopType.Priest:
                //TBD
                break;

            default:
                break;
        }

        #endregion Price Change Text
    }

    /// <summary>
    /// This is where the purchase logic for each item for the Blacksmith will stay
    /// </summary>

    #region Blacksmith Items

    public void BuySwordUpgrade(float damageIncrease) //Buy sword strength upgrade function
    {
        int purchaseCount = 0;
        if (GameManager.instance.TrySpendCoins(swordUpgradePrice))
        {
            //Play audio clip for purchasing
            player.PlayAudio(purchaseElement, audioSource, false);

            purchaseCount++;
            boughtUpgrade = true;
            GameManager.instance.firstUpgrade = true;

            sceneInfo.swordDamageValue += damageIncrease;

            sceneInfo.isWeaponUpgradeBought = true;

            //In case we want to have increasing price over time
            // Recalculate price and refresh text only when price changed

            swordUpgradePrice = 50;
            UpdatePrice();
        }
        else if (purchaseCount >= 2)
        {
            firstItemButton.interactable = false;
            swordUpgradePrice = 15;
        }
        else
        {
            Debug.Log("Not enough coins");
            player.PlayAudio(noMoneyElement, audioSource, false);
        }
    }

    public void BuyKnockbackUpgrade(float FORCE)
    {
        if (GameManager.instance.TrySpendCoins(knockbackUpgradePrice))
        {
            //Play audio clip for purchasing
            player.PlayAudio(purchaseElement, audioSource, false);

            boughtUpgrade = true;
            GameManager.instance.firstUpgrade = true;
            //buyEvent.Raise(this, knockbackUpgradePrice);         // keep notifying other systems
            sceneInfo.knockbackForce += FORCE;

            sceneInfo.isWeaponUpgradeBought = true;

            firstItemButton.interactable = false;
        }
        else
        {
            Debug.Log("Not enough coins");
            player.PlayAudio(noMoneyElement, audioSource, false);
        }
    }

    public void BuyAxe() //Function for buying the axe
    {
        int amount = GameManager.instance.currentCoin;
        if (GameManager.instance.TrySpendCoins(axePrice))
        {
            //If purchased, play audio cue
            player.PlayAudio(purchaseElement, audioSource, false);
            //buyEvent.Raise(this, axePrice);
            boughtAxe = true;
            sceneInfo.isAxeBought = true;
            secondItemEvent.Raise(this, true);

            EventSystem.current.SetSelectedGameObject(shopNext);

            PlayerPrefs.SetInt("AxeBought", 1);
            PlayerPrefs.Save();
        }
        else
        {
            player.PlayAudio(noMoneyElement, audioSource, false);
        }
    }

    #endregion Blacksmith Items

    /// <summary>
    /// This is where the purchase logic for each item in the Potion shop are held
    /// </summary>

    #region Alchemist Items

    public void BuyHealthPotion()
    {
        int amount = GameManager.instance.currentCoin;
        if (amount >= healthPrice)
        {
            player.PlayAudio(purchaseElement, audioSource, false);
            //buyEvent.Raise(this, healthPrice);

            sceneInfo.isHPBought = true;
        }
        else if (amount <= 0)
        {
            player.PlayAudio(noMoneyElement, audioSource, false);
        }
    }

    #endregion Alchemist Items

    public void CloseShop()
    {
        //Disable event system
        EventSystem.current.SetSelectedGameObject(null);

        shopUI.SetActive(false);

        bool boughtSword = GameManager.instance.firstUpgrade;

        GameManager.instance.StateSwitch(GameStates.Play);
        Invoke(nameof(ReEnablePlayer), 0.3f);
        if (!boughtAxe && !boughtUpgrade && shopType == ShopType.Blacksmith)
        {
            //Play audio clip if player hasn't bought anything from shop
            player.PlayAudio(leaveWithoutPayElement, audioSource, false);
        }
        else if (!boughtHealth && shopType == ShopType.Alchemist)
        {
            //Play audio clip if player hasn't bought anything from shop
            player.PlayAudio(leaveWithoutPayElement, audioSource, false);
        }
        else
        {
            return;
        }
    }

    private void ReEnablePlayer()
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