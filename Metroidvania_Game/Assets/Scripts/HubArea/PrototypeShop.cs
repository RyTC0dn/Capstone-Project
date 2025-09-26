using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypeShop : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    PrototypePlayerMovementControls playerMovementControls;
    public GameObject weapon;

    private int price = 2;

    [SerializeField]private bool isNearShop = false;
    public bool gotWeapon = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!isNearShop)
        {
            interactText.enabled = false;
        }
        
        playerMovementControls = FindFirstObjectByType<PrototypePlayerMovementControls>();
    }

    public void BuyFunction()
    {
        if(weapon != null && isNearShop && playerMovementControls.coinTracker > 0)
        {
            playerMovementControls.coinTracker -= price;
            gotWeapon = true;
            Destroy(weapon);
        }
       
    }

    public void Display(string hoverText)
    {
        interactText.text = hoverText;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText.enabled = true;
            isNearShop = true;
            string text = "Press E to Buy";
            Display(text);
        }
        else
        {
            isNearShop = false;
        }
    }
}
