using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script will handle what goes on within the individual tutorial scene
/// </summary>
public class TutorialScene : MonoBehaviour
{
    public GameObject tutorialSpawn;


    private void Awake()
    {
        TutorialManager.instance.LoadTutorialSpawn(tutorialSpawn);
    }

    private void Update()
    {
        bool input = Keyboard.current?.pKey.isPressed ?? false;

        if (input)
        {
            TutorialManager.instance.StartTutorial(tutorialSpawn);
        }
    }

}
