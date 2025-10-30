using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Text")]
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI weaponUpgradeText;

    [Header("HP Icon")]
    public List<Image> clockIcons = new List<Image>(); //Drag each UI Clock image in order
    public Sprite fullClockSprite;
    public Sprite brokenClockSprite;

    [SerializeField]private int totalHealth = 4;
    public int totalCoin = 0;
    [SerializeField]private int currentCoin;
    private int totalUpgradeLevel = 0;
    public int currentUpgrade;
    private int upgradeValue = 1;

    [Header("Pause Menu")]
    public GameObject pauseMenu;
    public static bool isGamePaused = false;

    private void Awake()
    {
        //Setting the current UI values with the total values
        currentCoin = totalCoin;
        currentUpgrade = totalUpgradeLevel;

        SetHealth(totalHealth);
        SetCoin(currentCoin);
        SetUpgrade(currentUpgrade);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
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

    private void SetUpgrade(int upgrade)
    {
        weaponUpgradeText.text = "+"+upgrade.ToString();
    }

    public void UpgradeUpdate(Component sender, object data)
    {
        if (data is bool bought)
        {
            Debug.Log($"{bought}");
            if (currentCoin > 0)
            {
                currentUpgrade += upgradeValue;
                SetUpgrade(currentUpgrade);
                Debug.Log($"{bought}");
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    void Pause()
    {
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGameInPause()
    {
        Application.Quit();
    }

}
