using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "Scriptable Objects/NPC")]
public class NPC : ScriptableObject
{
    /// <summary>
    /// This script is to define the NPCs that we find throughout the game
    /// by setting the name quickly apply to any object in the scene
    /// </summary>
    public string npcName;
}
