using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Text")]
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI coinText;

    [Header("HP Icon")]
    public List<Image> clockIcons = new List<Image>(); //Drag each UI Clock image in order
    public Sprite fullClockSprite;
    public Sprite brokenClockSprite;

    private int totalHealth = 4;
    private int totalCoin = 0;
    public int currentCoin;

    private void Awake()
    {
        currentCoin = totalCoin;
        SetHealth(totalHealth);
        SetCoin(currentCoin);
    }

    private void SetHealth(int health)
    {
        playerHealthText.text = "Player HP: ";

        //Each clock will represent 2 HP
        int remainingHealth = health;

        for (int i = 0; i < clockIcons.Count; i++)
        {
            //This is to ensure to change the sprites whether the sprites
            //are using sprite renderers or UI image components
            var image = clockIcons[i] as Image;
            var spriteRenderer = clockIcons[i].GetComponent<SpriteRenderer>();

            if(remainingHealth >= 2)
            {
                //Sprite for unbroken clock at 2 HP
                if (image) image.sprite = fullClockSprite;
                if(spriteRenderer) spriteRenderer.sprite = fullClockSprite;
                if (image) image.enabled = true;
                if (spriteRenderer) spriteRenderer.enabled = true;
                
                remainingHealth -= 2;
            }
            else if (remainingHealth == 1)
            {
                //Sprite for broken clock at 1 HP
                if (image) image.sprite = brokenClockSprite;
                if (spriteRenderer) spriteRenderer.sprite = brokenClockSprite;
                if (image) image.enabled = true;
                if (spriteRenderer) spriteRenderer.enabled = true;

                remainingHealth -= 1;
            }
            else
            {
                //When no health left > hide icon
                if(image) image.enabled = false;
                if(spriteRenderer) spriteRenderer.enabled = false;
            }
        }
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
            if (currentCoin > 0)
            {
                currentCoin -= amount;
                SetCoin(currentCoin);
            }
            
        }
    }

}
