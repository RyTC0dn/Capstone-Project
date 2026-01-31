using UnityEngine;
using UnityEngine.InputSystem;

public enum DoorType
{
    // For our purposes and separating the types of doors being used in scenes
    Standard,
    Cutscene
}

public class LeverSwitch : MonoBehaviour
{
    bool flippedSwitch = false;
    private bool isDetected = false;
    private bool wasFlipped = false;
    public GameEvent switchFlipEvent;
    public GameObject buttonPrompt;
    [SerializeField] private SpriteRenderer leverSP;
    [Tooltip("Assign a number in relation to which opening event is triggered, starting at 0")]
    [SerializeField] private int signalNumber;

    [SerializeField] private DoorType doorType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (buttonPrompt != null)
            buttonPrompt.SetActive(false);

        if (leverSP == null)
            leverSP = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        bool keyInput = Keyboard.current?.eKey.isPressed ?? false;
        bool buttonInput = Gamepad.current?.xButton.isPressed ?? false;

        if ((keyInput || buttonInput) && isDetected)
        {
            OpenDoor();
        }
    }

    // Door lever functions
    private void OpenDoor()
    {
        if (wasFlipped)
            return;

        wasFlipped = true;
        flippedSwitch = true;

        if (leverSP != null)
            leverSP.flipX = true;

        Debug.Log("Lever is flipped");

        #region Door Type
        if (switchFlipEvent == null)
            return;

        switch (doorType)
        {
            case DoorType.Standard:
                // Standard door open event
                switchFlipEvent.Raise(this, flippedSwitch);
                break;
            case DoorType.Cutscene:
                // Send signal based on which area is being opened up
                switchFlipEvent.Raise(this, signalNumber);
                break;
            default:
                break;
        }
        #endregion
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isDetected = true;
            buttonPrompt?.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isDetected = false;
            buttonPrompt?.SetActive(false);
        }
    }
}
