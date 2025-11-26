using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// This script is in need of revision
/// </summary>
public enum GameStates
{
    Play, 
    Pause
}

public class GameManager : MonoBehaviour
{
    public GameStates state;
    public static GameManager instance { get; private set; }

    private GameObject pauseMenu;
    CameraZones zones;

    [Header("SpawnPoint Settings")]
    public string nextSpawnPointName; //Storing the name of the different spawn points
    public GameObject[] playerSpawnPoint; // Stores the position that the player will teleport to when hit or start in scene

    //Checking if npcs are saved
    public bool isBlacksmithSaved = false;
    public bool isPotionMakerSaved = false;
    public bool isHealerSaved = false;

    [Header("Player UI Components")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI weaponUpgradeText;

    public int totalCoin = 0;
    [HideInInspector] public int currentCoin;
    private int totalUpgradeLevel = 0;
    public int currentUpgrade;
    private int upgradeValue = 1;

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

    //When player loses 2 hp
    public void SendPlayerToStart(Component sender, object data)
    {
        if (data is Transform player && sender is PlayerHealth)
        {
            //GameObject player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log("Teleport player");

            player.transform.position = zones.playerSpawnPoint.transform.position;
            
        }
    }

    //Coin collection function will be moved over to player UI script

    public void StateSwitch(GameStates state)
    {
        switch (state)
        {
            case GameStates.Pause:
                Time.timeScale = 0;
                break;
            case GameStates.Play:
                Time.timeScale = 1;
                break;
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            StateSwitch(GameStates.Pause);
            Debug.Log("Game is paused");
        }
    }

    /// <summary>
    /// All functions below are meant for the player UI
    /// </summary>
    /// <param name="coin"></param>
    private void SetCoin(int coin)
    {
        coinText.text = coin.ToString();
    }
    public void UpdateCoins(Component sender, object data)
    {
        if (data is int && sender is PrototypeShop)
        {
            int amount = (int)data;
            if (currentCoin > 0)
            {
                currentCoin -= amount;
                SetCoin(currentCoin);
            }

        }

        //Check for coin collection script event to add to coin count
        if (data is int && sender is CoinCollection)
        {
            int amount = (int)data;
            currentCoin += amount;
            SetCoin(currentCoin);
        }
    }

    private void SetUpgrade(int upgrade)
    {
        weaponUpgradeText.text = "+" + upgrade.ToString();
    }

    public void UpgradeUpdate(Component sender, object data)
    {
        if (data is bool bought)
        {
            if (bought)
            {
                currentUpgrade += upgradeValue;
                SetUpgrade(currentUpgrade);
                Debug.Log($"{bought}");
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Look for the UI objects by tag or name each time a scene loads
        coinText = GameObject.Find("CoinText")?.GetComponent<TextMeshProUGUI>();
        weaponUpgradeText = GameObject.Find("WeaponUpgradeText")?.GetComponent<TextMeshProUGUI>();

        // Refresh UI with current values
        if (coinText != null) SetCoin(currentCoin);
        if (weaponUpgradeText != null) SetUpgrade(currentUpgrade);

        zones = FindAnyObjectByType<CameraZones>();
    }
}
