using TMPro;
using UnityEngine;

/// <summary>
/// I just created this script to give some basic UI updating for the coin tracker
/// If you want to make any additional changes then please go ahead
/// </summary>
public class UIManager : MonoBehaviour
{
    //Game Variables
    public int startingCoins = 0;
    private int coinCount;
    private int lifeTracker;

    //Text mesh pro variables
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI playerHealthText;

    PrototypePlayerMovementControls playerControls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();

        coinCount = startingCoins;

        lifeTracker = playerControls.playerLives;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateUI()
    {
        coinText.text = "Coins: " + coinCount.ToString();
        playerHealthText.text = "Player Lives: " + lifeTracker.ToString();
    }

    public void PlayerLives()
    {
        lifeTracker--;
        UpdateUI();

        playerControls.gameObject.transform.position = playerControls.playerSpawnPoint.position;
    }

    public void CoinsCollected()
    {
        coinCount++;
        UpdateUI();
    }
}
