using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public int coinTracker;
    public int playerLives = 3;
    public int attackValue = 1;
    public int upgradeValue = 0;

    PrototypePlayerMovementControls playerMovementControls;
    UIManager ui;
    private AudioSource coinPing;

    private GameObject pauseMenu;

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
        playerMovementControls = FindAnyObjectByType<PrototypePlayerMovementControls>();
        ui = FindAnyObjectByType<UIManager>();  
        coinPing = GetComponent<AudioSource>();        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerLives <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        SceneManager.LoadScene("Town");
        playerLives = 3;
    }

    public void PlayCoinAudio() //Call this function when player collides with coin
    {
        coinPing.Play();    
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
    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            StateSwitch(GameStates.Pause);
            Debug.Log("Game is paused");
        }
    }
}
