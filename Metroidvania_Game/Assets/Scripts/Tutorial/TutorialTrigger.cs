using UnityEngine;

//This list will get updated as more tutorials are finished
public enum CurrentTutorial
{
    Movement, 
    UI
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
    [SerializeField]private int tutorialSequence;
    [SerializeField] private int maxSequence;

    private void Start()
    {
        tutorialSequence = TutorialManager.Instance.currentNotificationIndex;
        maxSequence = TutorialManager.Instance.notifications.textLines.Length;
    }

    private void Update()
    {
        // Update the tutorial sequence based on the current notification index from the TutorialManager
        tutorialSequence = TutorialManager.Instance.currentNotificationIndex;
    }

    //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            #region Movement Tutorial
            if(tutorial == CurrentTutorial.Movement
                && tutorialSequence != maxSequence) {
                TutorialManager.Instance.NextTutorialNotification();
                //MovementTutorial(tutorialSequence);
                gameObject.SetActive(false);                
            }
            #endregion
        }
    }

}
