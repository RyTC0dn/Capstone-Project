using TMPro;
using UnityEngine;

/// <summary>
/// I just created this script to give some basic UI updating for the coin tracker
/// If you want to make any additional changes then please go ahead
/// </summary>
public class UIManager : MonoBehaviour
{
    //Game Variables
    private int coinCount;
    public static UIManager instance;

    //Text mesh pro variables
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI swordAttackStatText;

    PrototypePlayerMovementControls playerControls;
    PrototypePlayerAttack playerAttack;
    //public GameObject pauseMenu;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();
        playerAttack = FindAnyObjectByType<PrototypePlayerAttack>();

        coinCount = GameManager.instance.coinTracker;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// This function is to store variables into text form and update 
    /// the UI respectively to each UI elements
    /// *This may get changed slightly with more finalized UI
    /// </summary>
    public void UpdateUI() 
    {
        coinText.text = "Coins: " + GameManager.instance.coinTracker.ToString();
        playerHealthText.text = "Player Lives: " + GameManager.instance.playerLives.ToString();
        swordAttackStatText.text = "+" + playerAttack.upgradeValue.ToString();
    }

    public void PlayerLives()
    {
        GameManager.instance.playerLives--;
        UpdateUI();

        GameManager.instance.gameObject.transform.position = playerControls.playerSpawnPoint.position;
    }

    public void Upgrade(int price)
    {
        playerAttack.upgradeValue++;
        GameManager.instance.coinTracker -= price;
        UpdateUI();
    }

    public void CoinsCollected()
    {
        GameManager.instance.coinTracker++;
        UpdateUI();
    }
}
