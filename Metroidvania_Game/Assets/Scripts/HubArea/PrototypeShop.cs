using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypeShop : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    PrototypePlayerMovementControls playerMovementControls;
    PrototypePlayerAttack playerAttack;
    public GameObject shopUI;
    private UIManager uiManager;
    public static GameManager gm; //Adding the game manager here so that I can use a pause states on the game

    private int upgradePrice = 8;

    public bool isNearShop = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {              
        playerMovementControls = FindFirstObjectByType<PrototypePlayerMovementControls>();
        playerAttack = FindFirstObjectByType<PrototypePlayerAttack>();
        uiManager = FindAnyObjectByType<UIManager>();
        interactText.enabled = isNearShop;
        shopUI.SetActive(false);

        gm = GameManager.instance; //Calling the gamemanager 
    }

    private void Update()
    {
        
    }

    //This function is being called by the player movement controls script
    public void EnableShop() ///This is for general shopping interactions 
    {
        //Set the shop ui object to active when function is called
        shopUI.SetActive(true);
        gm.state = GameStates.Pause; //The game will pause but the player can still "Attack" during it
    }

    public void BuySwordUpgrade()
    {
        if(playerMovementControls.coinTracker >= 8)
        {
            uiManager.Upgrade();
            playerMovementControls.coinTracker -= upgradePrice;
            Debug.Log("Bought Upgrade");
        }        
    }

    public void Display(string hoverText)
    {
        interactText.text = hoverText;
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.CompareTag("Player"))
        {  
            isNearShop = true;
            if(isNearShop)
            {
                string text = "Press E to Interact";
                Display(text);
            }
 
        }
    }
}
