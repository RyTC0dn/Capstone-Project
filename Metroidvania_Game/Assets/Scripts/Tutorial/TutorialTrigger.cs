using UnityEngine;

/// <summary>
/// Attach this script onto game object that will act as 
/// a trigger for the next tutorial sequence.
/// </summary>

//This list will get updated as more tutorials are finished
public enum CurrentTutorial
{
    Movement, 
    UI
}

public class TutorialTrigger : MonoBehaviour
{
    public GameEvent tutorialEvent;
    public SceneInfo sceneInfo;
    public CurrentTutorial tutorial;
    [SerializeField]private int tutorialSequence;
    [SerializeField] private int maxSequence;

    //Main event trigger logic that will call other functions
    public void Trigger(Component sender, object data)
    {
        //Send tutorial event
        tutorialEvent.Raise(sender, data);
    }

    //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            #region Movement Tutorial
            if(tutorial == CurrentTutorial.Movement
                && tutorialSequence != maxSequence) {
                tutorialSequence++;
                MovementTutorial(tutorialSequence);
                if(tutorialSequence >= maxSequence)
                {
                    tutorialSequence = 0;
                    return;
                }
            }
            #endregion
        }
    }

    private void MovementTutorial(int sequence)
    {
        if(sequence == 1)
        {
            Trigger(this, sceneInfo.isMoved);
            Debug.Log("Movement");
        }
        else if(sequence == 2)
        {
            Trigger(this, sceneInfo.isJumped);
            Debug.Log("Jumped");
        }
    }

}
