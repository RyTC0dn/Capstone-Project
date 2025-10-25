using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        SetHealth(4);
        SetCoin(0);
    }

    private void SetHealth(int health)
    {
        playerHealthText.text = "Player Lives: " + health.ToString();
    }

    public void UpdateHealth(Component sender, object data)
    {
        //If sender is PlayerHealth
        if(data is int)
        {
            int amount = (int)data;
            SetHealth(amount);
            Debug.Log($"Update health by {amount}");
        }        
    }

    private void SetCoin(int coin)
    {
        coinText.text = "Coins: " + coin.ToString();
    }
    public void UpdateCoins(Component sender, object data)
    {
        if (data is int)
        {
            int amount = (int)data;
            SetCoin(amount);
        }
    }

}
