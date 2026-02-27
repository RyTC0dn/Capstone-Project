using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Scriptable Objects/SceneInfo")]
public class SceneInfo : ScriptableObject
{
    [Header("Abilities")]
    public bool isShieldPickedUp = false;
    public bool isWallBreakPickedUp = false;
    [Space(20)]

    [Header("Shop")]
    public bool isAxeBought = false;
    public bool isHPBought = false;
    [Space(20)]

    [Header("Cutscenes")]
    public bool isBossCutscenePlayed = false;
    [Space(20)]
    
    [Header("Tutorials")]
    //Movement Tutorial
    public bool isMoved = false;
    public bool isJumped = false;
}
