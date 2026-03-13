using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Represents a trigger component that manages and raises tutorial events based on player interactions within a scene.
/// </summary>
/// <remarks>Attach this component to a GameObject to handle tutorial progression and event triggering in response
/// to player actions, such as entering specific areas. The component coordinates with associated tutorial data and
/// scene information to determine when to raise tutorial events. Ensure that the required references to tutorial event,
/// scene information, and tutorial type are assigned for correct operation.</remarks>
public class TutorialTrigger : MonoBehaviour
{
    private bool isNearby = false;
    private bool eventTriggered = false;
    private bool input;
    [SerializeField] private int tutorialSequence;
    public GameObject[] triggers; //Mainly for setting up the
    public SceneInfo sceneInfo;
    public GameEvent tutorialTriggerEvent;
    public TextMeshProUGUI starText;
    public TutorialHandler tutorialHandler;

    private void Start()
    {
        tutorialSequence = tutorialHandler.stepIndex;
        starText.text = tutorialSequence.ToString();

        //Eventually have all tutorial scenes have the show tutorial text, but for now just have it for the movement tutorial
    }

    private void Update()
    {
        triggers[tutorialSequence].SetActive(input);
    }

    //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && tutorialHandler.type == TutorialType.Movement)
        {
            tutorialHandler.NextStep();
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Player") && tutorialHandler.type == TutorialType.Dash)
        {
            tutorialHandler.NextStep();
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Player") && tutorialHandler.type == TutorialType.NPC)
        {
            tutorialHandler.NextStep();
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Player")
            && tutorialHandler.type == TutorialType.NPC
            && tutorialHandler.stepIndex == 3)
        {
            tutorialHandler.OnNPCFinale(true);
        }
    }

    public void NextMessage()
    {
        TutorialManager.Instance.NextTutorialNotification();
    }
}