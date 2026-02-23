using TMPro;
using UnityEngine;

public class TutorialMessages : MonoBehaviour
{
    public GameEvent tutorialEvents;
    private bool isTriggered = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        isTriggered = true;

        if (collision.CompareTag("Player"))
        {
            tutorialEvents.Raise(this, isTriggered);
        }
    }
}
