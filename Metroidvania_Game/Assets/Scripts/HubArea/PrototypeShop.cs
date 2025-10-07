using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypeShop : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    PrototypePlayerMovementControls playerMovementControls;
    public GameObject weapon;

    private int price = 2;

    [SerializeField]private bool isNearShop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {              
        playerMovementControls = FindFirstObjectByType<PrototypePlayerMovementControls>();
        isNearShop = false;
    }

    private void Update()
    {
        if (!isNearShop)
        {
            interactText.enabled = false;
        }
    }

    public void BuyFunction()
    {
        if(weapon != null && isNearShop && playerMovementControls.coinTracker > 0)
        {
            playerMovementControls.coinTracker -= price;
            Destroy(weapon);
        }
       
    }

    public void Display(string hoverText)
    {
        interactText.text = hoverText;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isNearShop = collision.CompareTag("Player");
        if (isNearShop)
        {
            interactText.enabled = true;
            string text = "Press E to Buy";
            Display(text);
        }
    }
}
