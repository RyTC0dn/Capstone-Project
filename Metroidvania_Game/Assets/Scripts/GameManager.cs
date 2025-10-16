using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    [Header("Player UI Stats")]
    private int coinTracker = 0;
    public int currentCoins;
    private int playerLives = 3;
    public int currentLives;
    public int upgradeValue = 0;

    UIManager ui;
    public AudioSource coinPickup;
    public AudioSource coinPouch;
    private float timer = 5;

    private GameObject pauseMenu;

    public bool hasSavedBlacksmith = false;

    [Header("SpawnPoint Settings")]
    public string nextSpawnPointName; //Storing the name of the different spawn points

    public Dictionary<string, bool> npcSaved = new Dictionary<string, bool>();

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
        //Initializing scripts within the game manager
        ui = FindAnyObjectByType<UIManager>();  

        currentLives = playerLives;
        currentCoins = coinTracker;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentLives <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        SceneManager.LoadScene("Town");
        currentLives = playerLives;
    }

    public void PlayerDamaged(int enemyDamage)
    {
        currentLives -= enemyDamage;
        ui.UpdateUI();
    }

    public void CoinCollection(int gainAmount)
    {
        currentCoins += gainAmount;
        ui.UpdateUI();
    }

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

    public bool IsNPCSaved(string npcName)
    {
        return npcSaved.ContainsKey(npcName) && npcSaved[npcName];
    }

    public void SetNPCSaved(string npcName, bool saved)
    {
        npcSaved[npcName] = saved;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            StateSwitch(GameStates.Pause);
            Debug.Log("Game is paused");
        }
    }
}
