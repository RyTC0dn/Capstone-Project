using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// This script is to help initialize input functions with UI components
/// </summary>
/// 

///This script is in need of revision
public class UIManager : MonoBehaviour
{
    //Game Variables
    private int coinCount;
    public static UIManager instance { get; private set; }

    PrototypePlayerMovementControls playerControls;
    PrototypePlayerAttack playerAttack;
    public GameObject player;

    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject keyboardControlMenu;
    public GameObject gamepadControlMenu;
    public static bool isGamePaused = false;

    [Header("First Selected Option")]
    [SerializeField] private GameObject menuFirst;
    [SerializeField] private GameObject settingsMenuFirst;
    [SerializeField] private GameObject startMenuFirst;
    [SerializeField] private GameObject controlMenuFirst;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        string checkSceneName = SceneManager.GetActiveScene().name;
        if(checkSceneName == "StartMenu")
        {
            EventSystem.current.SetSelectedGameObject(startMenuFirst);
        }

        //Deactivate control menus on awake
        keyboardControlMenu.SetActive(false);
        gamepadControlMenu.SetActive(false);
        controlMenuFirst.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();
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

    void FindPlayer()
    {
        if (playerControls != null && playerAttack != null)
            return;

        var p = GameObject.FindWithTag("Player");

        if (p != null)
        {
            playerControls = p.GetComponent<PrototypePlayerMovementControls>();
            playerAttack = p.GetComponent<PrototypePlayerAttack>();
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
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene("Town");
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
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

        EventSystem.current.SetSelectedGameObject(null);

        if (playerControls != null)
            playerControls.enabled = true;
        if (playerAttack != null)
            playerAttack.enabled = true;
    }

    void Pause()
    {
        pauseMenu.gameObject.SetActive(true);
        settingsMenu.gameObject.SetActive(false);
        keyboardControlMenu.gameObject.SetActive(false);
        gamepadControlMenu.gameObject.SetActive(false);
        controlMenuFirst.gameObject.SetActive(false);
        Time.timeScale = 0f;
        isGamePaused = true;

        EventSystem.current.SetSelectedGameObject(menuFirst);

        //Deactivate player controls
        if (playerControls != null)
            playerControls.enabled = false;
        if (playerAttack != null)
            playerAttack.enabled = false;


    }

    #region Settings
    void OpenSettingsMenuHandle()
    {
        settingsMenu.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        keyboardControlMenu.gameObject.SetActive(false);
        gamepadControlMenu.gameObject.SetActive(false);
        controlMenuFirst.gameObject.SetActive(false);

        EventSystem.current.SetSelectedGameObject(settingsMenuFirst);
    }

    public void OpenKeyboardControls()
    {
        keyboardControlMenu.gameObject.SetActive(true);
        settingsMenu.gameObject.SetActive(false);
        controlMenuFirst.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(controlMenuFirst);
    }

    public void OpenGamepadControls()
    {
        gamepadControlMenu.gameObject.SetActive(true);
        settingsMenu.gameObject.SetActive(false);
        controlMenuFirst.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(controlMenuFirst);
    }
    #endregion

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
