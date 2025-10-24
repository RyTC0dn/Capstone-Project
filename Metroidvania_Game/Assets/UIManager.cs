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

    //Text mesh pro variables
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI swordAttackStatText;


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
        SetPlayerHealth(4);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// This function is to store variables into text form and update 
    /// the UI respectively to each UI elements
    /// *This may get changed slightly with more finalized UI
    /// </summary>
    public void UpdateUI() 
    {
        coinText.text = "Coins: " + GameManager.instance.currentCoins.ToString();
        //Add text here to update when player health changes
        swordAttackStatText.text = "+" + GameManager.instance.upgradeValue.ToString();
    }

    private void SetPlayerHealth(int health)
    {
        playerHealthText.text = "Player Lives: " + health.ToString();
    }

    public void UpdatePlayerHealth(Component sender, object data)
    {
        //If (sender is PlayerHealth)
        if(data is int)
        {
            int amount = (int)data;
            SetPlayerHealth(amount);
        }        
    } 

    public void Upgrade(int price)
    {
        GameManager.instance.upgradeValue++;
        GameManager.instance.currentCoins -= price;
        UpdateUI();
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
