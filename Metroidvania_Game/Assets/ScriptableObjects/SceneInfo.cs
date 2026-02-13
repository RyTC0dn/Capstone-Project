using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Scriptable Objects/SceneInfo")]
public class SceneInfo : ScriptableObject
{
    //Check if the abilities have been picked up
    public bool isShieldPickedUp = false;
    public bool isWallBreakPickedUp = false;
    public bool isAxeBought = false;
    public bool isHPBought = false;
}
