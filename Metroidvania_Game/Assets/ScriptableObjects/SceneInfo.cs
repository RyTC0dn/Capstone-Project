using UnityEngine;
using UnityEngine.InputSystem;

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
    public bool bookIsLookedAt = false;
    public bool isMoved = false;

    //Detect if the player is using a controller or keyboard
    public bool isController = false;
    public bool isKeyboard = false;

    public void ResetSceneInfo()
    {
        isShieldPickedUp = false;
        isWallBreakPickedUp = false;
        isAxeBought = false;
        isHPBought = false;
        isBossCutscenePlayed = false;
        bookIsLookedAt = false;
        isMoved = false;
        isController = false;
        isKeyboard = false;
    }

    //Detect if the player is using a controller or keyboard
    public bool OnDeviceChange(InputDevice device)
    {
        if(device is Gamepad)
        {
            return true;
        }
        else if (device is Keyboard)
        {
            return false;
        }
        return false;
    }
}
