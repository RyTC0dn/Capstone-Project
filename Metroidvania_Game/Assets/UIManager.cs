using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// I just created this script to give some basic UI updating for the coin tracker
/// If you want to make any additional changes then please go ahead
/// </summary>
/// 

///This script is in need of revision
public class UIManager : MonoBehaviour
{
    //Game Variables
    private int coinCount;
    public static UIManager instance {  get; private set; }

    PrototypePlayerMovementControls playerControls;
    PrototypePlayerAttack playerAttack;
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

    }

    /// <summary>
    /// This portion of the code will be dedicated to the start menu 
    /// </summary>
    public void CloseGame() //This will be called in the start menu screen
    {
        Application.Quit(); //*Will only be in effect during builds*
    }

    public void StartGame() //This will be called in the Start menu screen
    {
        SceneManager.LoadScene("Town");
    }


    //public void PauseMenu() //This function will work to 
    //{
    //    if(Keyboard.current.escapeKey.isPressed)
    //    {
    //        GameManager.instance.OnPause();
    //    }
    //}





}
