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

    //Text mesh pro variables
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI swordAttackStatText;

    PrototypePlayerMovementControls playerControls;
    PrototypePlayerAttack playerAttack;
    //public GameObject pauseMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();
        playerAttack = FindAnyObjectByType<PrototypePlayerAttack>();

        coinCount = playerControls.coinTracker;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateUI()
    {
        coinText.text = "Coins: " + playerControls.coinTracker.ToString();
        playerHealthText.text = "Player Lives: " + playerControls.playerLives.ToString();
        swordAttackStatText.text = "+" + playerAttack.upgradeValue.ToString();
    }

    public void PlayerLives()
    {
        playerControls.playerLives--;
        UpdateUI();

        playerControls.gameObject.transform.position = playerControls.playerSpawnPoint.position;
    }

    public void Upgrade(int price)
    {
        playerAttack.upgradeValue++;
        playerControls.coinTracker -= price;
        UpdateUI();
    }

    public void CoinsCollected()
    {
        playerControls.coinTracker++;
        UpdateUI();
    }
}
