using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypeShop : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    PrototypePlayerMovementControls playerMovementControls;
    public GameObject shopUI;
    public static GameManager gm;

    private int price = 8;

    public bool isNearShop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {              
        playerMovementControls = FindFirstObjectByType<PrototypePlayerMovementControls>();
        isNearShop = false;
        shopUI.SetActive(false);

        gm = GameManager.instance;
    }

    private void Update()
    {
        if (!isNearShop)
        {
            interactText.gameObject.SetActive(false);
        }
    }

    public void EnableShop() ///This is for general shopping interactions 
    {
        //Set the shop ui object to active when function is called
        shopUI.SetActive(true);
        gm.state = GameStates.Pause;
    }

    public void BuyFunction()
    {
        Debug.Log("Bought Weapon!");
        playerMovementControls.coinTracker -= price;
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
        //isNearShop = collision.CompareTag("Player");
        //if (isNearShop)
        //{
        //    interactText.enabled = true;
        //    string text = "Press E to Interact";
        //    Display(text);
        //}
    }
}
