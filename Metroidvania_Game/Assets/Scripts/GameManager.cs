using System.Collections;
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
    public int coinTracker;
    public int playerLives = 3;
    public int attackValue = 1;
    public int upgradeValue = 0;

    PrototypePlayerMovementControls playerMovementControls;
    UIManager ui;
    public AudioSource coinPickup;
    public AudioSource coinPouch;
    private float timer = 5;

    private GameObject pauseMenu;

    public bool hasSavedBlacksmith = false;

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
        coinPickup.Play();
        StartCoroutine(AudioDelay());
        if(timer <= 0)
            coinPouch.Play();
    }

    IEnumerator AudioDelay()
    {
        yield return new WaitForSeconds(0.5f);
        timer--;
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
