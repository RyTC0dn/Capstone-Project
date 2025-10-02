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
    public static GameManager instance { get; set; }

    PrototypePlayerMovementControls playerMovementControls;
    UIManager ui;
    private AudioSource coinPing;

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
        if(playerMovementControls.playerLives <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        SceneManager.LoadScene("Town");
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
}
