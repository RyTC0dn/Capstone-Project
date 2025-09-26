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

    //Text mesh pro variables
    public TextMeshProUGUI coinText;

    PrototypePlayerMovementControls playerControls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();

        coinCount = startingCoins;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateUI()
    {
        coinText.text = "Coins: " + coinCount.ToString();
    }

    public void CoinsCollected()
    {
        coinCount++;
        UpdateUI();
    }
}
