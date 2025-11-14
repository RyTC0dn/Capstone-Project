using UnityEngine;
using UnityEngine.InputSystem;

public class LeverSwitch : MonoBehaviour
{
    bool flippedSwitch = false;
    private bool isDetected = false;
    private bool wasFlipped = false;
    public GameEvent switchFlipEvent;
    public GameObject buttonPrompt;
    [SerializeField]private SpriteRenderer leverSP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonPrompt.SetActive(false);
        leverSP = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.isPressed && isDetected)
        {
            OpenDoor();
        }
    }

    //Door lever functions
    private void OpenDoor()
    {
        flippedSwitch = true;
        leverSP.flipX = true;
        Debug.Log("Lever is flipped");
        switchFlipEvent.Raise(this, flippedSwitch);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
           isDetected = true;
            buttonPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isDetected = false;
            buttonPrompt.SetActive(false);
        }
    }
}
