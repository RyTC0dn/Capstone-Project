using UnityEngine;
using UnityEngine.InputSystem;

public enum GameStates
{
    Play, 
    Pause
}

public class GameManager : MonoBehaviour
{
    public GameStates state;
    public static GameManager instance { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StateSwitch()
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
