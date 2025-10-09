using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Line
{
    public NPC npc;

    [TextArea(2, 5)]
    public string text;
}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue/Tower")]
public class Dialogue : ScriptableObject
{
    public Line[] textLines;
    public AudioClip[] audioVoice;
}
