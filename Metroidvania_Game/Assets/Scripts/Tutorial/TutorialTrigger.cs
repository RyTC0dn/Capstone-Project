using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//This list will get updated as more tutorials are finished
public enum CurrentTutorial
{
    Movement,
    UI,
    Wallbreak,
    Combat,
    NPCInteraction,
    DoorInteraction,
    None
}

/// <summary>
/// Represents a trigger component that manages and raises tutorial events based on player interactions within a scene.
/// </summary>
/// <remarks>Attach this component to a GameObject to handle tutorial progression and event triggering in response
/// to player actions, such as entering specific areas. The component coordinates with associated tutorial data and
/// scene information to determine when to raise tutorial events. Ensure that the required references to tutorial event,
/// scene information, and tutorial type are assigned for correct operation.</remarks>
public class TutorialTrigger : MonoBehaviour
{
    public CurrentTutorial tutorial;
    [SerializeField] private int tutorialSequence;
    [SerializeField] private int maxSequence;
    public SceneInfo sceneInfo;

    private void Start()
    {
        tutorialSequence = TutorialManager.Instance != null ? TutorialManager.Instance.currentNotificationIndex : 0;
        maxSequence = (TutorialManager.Instance != null && TutorialManager.Instance.notifications != null)
            ? TutorialManager.Instance.notifications.textLines.Length : 0;

        //Eventually have all tutorial scenes have the show tutorial text, but for now just have it for the movement tutorial
    }

    private void Update()
    {
        // Update the tutorial sequence based on the current notification index from the TutorialManager
        tutorialSequence = TutorialManager.Instance.currentNotificationIndex;
        if (tutorialSequence >= maxSequence)
        {
            #region Complete Tutorial

            switch (tutorial)
            {
                case CurrentTutorial.Movement:
                    TutorialManager.Instance.ShowTutorialText(false);
                    TutorialManager.Instance.currentNotificationIndex = 0;
                    sceneInfo.isMoved = true;
                    TutorialManager.Instance.SendBackToLevel();
                    break;

                case CurrentTutorial.UI:
                    TutorialManager.Instance.ShowTutorialText(false);
                    TutorialManager.Instance.currentNotificationIndex = 0;
                    sceneInfo.bookIsLookedAt = true;
                    TutorialManager.Instance.SendBackToLevel();
                    break;

                case CurrentTutorial.Wallbreak:
                    TutorialManager.Instance.ShowTutorialText(false);
                    TutorialManager.Instance.currentNotificationIndex = 0;
                    sceneInfo.dashed = true;
                    TutorialManager.Instance.SendBackToLevel();
                    break;

                case CurrentTutorial.Combat:
                    TutorialManager.Instance.ShowTutorialText(false);
                    TutorialManager.Instance.currentNotificationIndex = 0;
                    sceneInfo.combat = true;
                    TutorialManager.Instance.SendBackToLevel();
                    break;

                case CurrentTutorial.NPCInteraction:
                    TutorialManager.Instance.ShowTutorialText(false);
                    TutorialManager.Instance.currentNotificationIndex = 0;
                    sceneInfo.npcInteracted = true;
                    TutorialManager.Instance.SendBackToLevel();
                    break;

                case CurrentTutorial.DoorInteraction:
                    TutorialManager.Instance.ShowTutorialText(false);
                    TutorialManager.Instance.currentNotificationIndex = 0;
                    sceneInfo.door = true;
                    TutorialManager.Instance.SendBackToLevel();
                    break;

                case CurrentTutorial.None:
                    break;

                default:
                    break;
            }

            #endregion Complete Tutorial
        }
        else
        {
            if (tutorial == CurrentTutorial.Combat)
            {
                //Have the enemies appear in sequence of the tutorial
                TutorialManager.Instance.enemies[tutorialSequence].SetActive(true);
            }
            else
            {
                TutorialManager.Instance.enemies[tutorialSequence].SetActive(false);
            }
        }
    }

    //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            #region Assigning tutorial logic

            switch (tutorial)
            {
                case CurrentTutorial.Movement:
                    TutorialManager.Instance.NextTutorialNotification();
                    //MovementTutorial(tutorialSequence);
                    gameObject.SetActive(false);
                    break;

                case CurrentTutorial.UI:
                    TutorialManager.Instance.NextTutorialNotification();
                    //MovementTutorial(tutorialSequence);
                    gameObject.SetActive(false);
                    break;

                case CurrentTutorial.Wallbreak:
                    TutorialManager.Instance.NextTutorialNotification();
                    //MovementTutorial(tutorialSequence);
                    gameObject.SetActive(false);
                    break;

                case CurrentTutorial.Combat:
                    TutorialManager.Instance.NextTutorialNotification();
                    //MovementTutorial(tutorialSequence);
                    gameObject.SetActive(false);
                    break;

                case CurrentTutorial.NPCInteraction:
                    TutorialManager.Instance.NextTutorialNotification();
                    //MovementTutorial(tutorialSequence);
                    gameObject.SetActive(false);
                    break;

                case CurrentTutorial.DoorInteraction:
                    TutorialManager.Instance.NextTutorialNotification();
                    //MovementTutorial(tutorialSequence);
                    gameObject.SetActive(false);
                    break;

                case CurrentTutorial.None:
                    break;

                default:
                    break;
            }

            #endregion Assigning tutorial logic
        }
    }

    public void NextMessage()
    {
        TutorialManager.Instance.NextTutorialNotification();
    }

    public void SendBackToLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level 1");
    }
}