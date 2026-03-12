using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialTriggerEvents : MonoBehaviour
{
    public CurrentTutorial tutorial;
    private bool isNearby = false;
    private bool eventTriggered = false;
    private bool input;
    [SerializeField] private int tutorialSequence;
    [SerializeField] private int maxSequence;
    public SceneInfo sceneInfo;
    public GameEvent tutorialTriggerEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        bool key = Keyboard.current?.eKey.wasPressedThisFrame ?? false;
        bool button = Gamepad.current?.buttonWest.wasPressedThisFrame ?? false;

        input = key || button;
        switch (tutorial)
        {
            case CurrentTutorial.Movement:
                if (!isNearby)
                    return;

                Debug.Log("Event triggered");

                eventTriggered = true;
                tutorialTriggerEvent.Raise(this, eventTriggered);
                gameObject.SetActive(false);
                break;

            case CurrentTutorial.UI:
                break;

            case CurrentTutorial.Wallbreak:
                break;

            case CurrentTutorial.Combat:
                break;

            case CurrentTutorial.NPCInteraction:
                break;

            case CurrentTutorial.DoorInteraction:
                if (!isNearby)
                    return;

                if (input)
                {
                    Debug.Log("Event triggered");

                    eventTriggered = true;
                    tutorialTriggerEvent.Raise(this, eventTriggered);
                    eventTriggered = false;
                }

                break;

            case CurrentTutorial.None:
                break;

            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNearby = true;
            Debug.Log("Player near");
        }
    }
}