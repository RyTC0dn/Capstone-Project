using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStep", menuName = "Tutorial/TutorialStep")]
public class TutorialStep : ScriptableObject
{
    [TextArea(3, 5)]
    public string dialogueText;

    public int arrowIndex = -1;

    public TutorialCondition condition;
}