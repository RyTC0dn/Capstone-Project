using UnityEngine;
using UnityEngine.InputSystem;

public class SaveSurvivor : MonoBehaviour
{
    [SerializeField]private bool playerIsNear = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame && playerIsNear)
        {
            Debug.Log("Rescued Survivor!");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {       

        if (collision.CompareTag("Player"))
        {
            playerIsNear = true;
           
        }
    }
}
