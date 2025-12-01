using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public struct Info
{
    [TextArea(2, 5)]
    public string text;
}

[CreateAssetMenu(fileName = "TutorialPrompts", menuName = "Scriptable Objects/TutorialPrompts")]
public class TutorialPrompts : ScriptableObject
{
    public Info[] textLines;
}
