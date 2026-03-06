using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Scriptable Objects/SceneInfo")]
public class SceneInfo : ScriptableObject
{
    [Header("Abilities")]
    public bool isShieldPickedUp = false;

    public bool isWallBreakPickedUp = false;
    public bool isShieldUsed = false;
    public bool isWallBreakUsed = false;

    [Space(20)]
    [Header("Shop")]
    public bool isAxeBought = false;

    public bool isHPBought = false;
    public bool isAxeUsed = false;

    [Space(20)]
    [Header("Cutscenes")]
    public bool isBossCutscenePlayed = false;

    [Space(20)]
    [Header("Tutorials")]
    public bool bookIsLookedAt = false;

    public bool isMoved = false;
    public bool npcInteracted = false;
    public bool combat = false;
    public bool dashed = false;
    public bool door = false;

    [Space(20)]
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
        if (device is Gamepad)
        {
            return true;
        }
        else if (device is Keyboard)
        {
            return false;
        }
        return false;
    }

    public void ResetSaveData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}