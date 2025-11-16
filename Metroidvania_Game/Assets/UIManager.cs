using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public static bool isGamePaused = false;

    [Header("First Selected Option")]
    [SerializeField]private EventSystem events;
    [SerializeField] private GameObject menuFirst;
    [SerializeField] private GameObject settingsMenuFirst;

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.MenuOpenCloseInput)
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


    /// <summary>
    /// This portion of the code will be dedicated to the start menu 
    /// </summary>
    public void CloseGame() //This will be called in the start menu screen
    {
        //Delete all saved data on close
        PlayerPrefs.DeleteAll();

        //Make sure changes are saved to disk
        PlayerPrefs.Save();

        Application.Quit(); //*Will only be in effect during builds*
    }

    public void StartGame() //This will be called in the Start menu screen
    {
        SceneManager.LoadScene("Town");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("StartMenu");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

    }


    public void Resume()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

        playerControls.enabled = true;
        playerAttack.enabled = true;

        EventSystem.current.SetSelectedGameObject(null);
    }

    void Pause()
    {
        pauseMenu.gameObject.SetActive(true);
        settingsMenu.gameObject.SetActive(false);
        Time.timeScale = 0f;
        isGamePaused = true;

        //Deactivate player controls
        playerAttack.enabled = false;
        playerControls.enabled = false;

        EventSystem.current.SetSelectedGameObject(menuFirst);
    }

    void OpenSettingsMenuHandle()
    {
        settingsMenu.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);

        EventSystem.current.SetSelectedGameObject(settingsMenuFirst);
    }

    #region Main Menu Button Actions
    public void OnSettingsPress()
    {
        OpenSettingsMenuHandle();
    }

    public void OnSettingsBackPress()
    {
        Pause();
    }

    #endregion
}
