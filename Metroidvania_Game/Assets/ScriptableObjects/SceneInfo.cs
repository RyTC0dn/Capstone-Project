using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Scriptable Objects/SceneInfo")]
public class SceneInfo : ScriptableObject
{
    //Abilities
    public bool isShieldPickedUp = false;
    public bool isWallBreakPickedUp = false;
    public bool isShieldUsed = false;
    public bool isWallBreakUsed = false;
    [Space(20)]

    //Shop
    public bool isAxeBought = false;
    public bool isHPBought = false;
    public bool isAxeUsed = false;  
    [Space(20)]

    //Cutscenes
    public bool isBossCutscenePlayed = false;
}
