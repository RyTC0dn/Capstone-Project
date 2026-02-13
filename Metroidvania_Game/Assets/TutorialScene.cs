using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script will handle what goes on within the individual tutorial scene
/// </summary>
public class TutorialScene : MonoBehaviour
{
    public GameObject tutorialSpawn;
    public GameObject playerTutorial;
    bool tutorialStart = false;


    private void Awake()
    {
        playerTutorial.SetActive(false);
    }

    private void Update()
    {
        EnablePlayer();        
    }

    private void EnablePlayer()
    {
        playerTutorial.SetActive(tutorialStart);
    }

}
