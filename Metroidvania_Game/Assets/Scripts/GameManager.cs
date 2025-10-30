using System.Collections;
using System.Collections.Generic;
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

    [Header("Audio")]
    UIManager ui;
    public AudioSource coinPickup;
    public AudioSource coinPouch;
    private float timer = 5;


    private GameObject pauseMenu;

    public bool hasSavedBlacksmith = false;

    [Header("SpawnPoint Settings")]
    public string nextSpawnPointName; //Storing the name of the different spawn points
    public GameObject playerSpawnPoint; // Stores the position that the player will teleport to when hit or start in scene

    public bool isNPCSaved = false;

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
        //coin stuff moved to playerUI
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //When player loses 2 hp
    public void SendPlayerToStart(Component sender, object data)
    {
        if (data is Transform && playerSpawnPoint != null && sender is PlayerHealth)
        {
            //GameObject player = GameObject.FindGameObjectWithTag("Player");
            Transform player = (Transform)data;
            if (player)
            {
                Debug.Log("Teleport player");
                player.transform.position = playerSpawnPoint.transform.position;
            }
            
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
}
