using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Scriptable Objects/SceneInfo")]
public class SceneInfo : ScriptableObject
{
    //Abilities
    public bool isShieldPickedUp = false;
    public bool isWallBreakPickedUp = false;
    [Space(20)]

    //Shop
    public bool isAxeBought = false;
    public bool isHPBought = false;
    [Space(20)]

    //Cutscenes
    public bool isBossCutscenePlayed = false;
}
