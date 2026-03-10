using UnityEngine;

[CreateAssetMenu(fileName = "TutorialSequence", menuName = "Tutorial/TutorialSequence")]
public class TutorialSequence : ScriptableObject
{
    public TutorialStep[] steps;
}