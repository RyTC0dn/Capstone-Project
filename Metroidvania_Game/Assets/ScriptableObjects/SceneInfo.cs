using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Represents the persistent state and progress of a game scene, including player abilities, shop purchases, cutscene
/// triggers, tutorial interactions, and enemy encounters.
/// </summary>
/// <remarks>Use this ScriptableObject to track and manage various gameplay elements and player interactions
/// within a specific scene. SceneInfo enables saving and resetting scene-specific data, ensuring consistent game state
/// management across play sessions. It is typically used to determine which events have occurred, which items have been
/// acquired or used, and to control the flow of tutorials and cutscenes. This class is intended to be edited via the
/// Unity Inspector or through game logic at runtime.</remarks>
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

    public bool isWeaponUpgradeBought = false;

    public bool isHPBought = false;
    public bool isAxeUsed = false;
    public float swordDamageValue = 1;
    public float knockbackForce = 1;

    //Call after first talking to NPCs to prevent the reminder popup
    //from reapperring
    public bool talkedToBlacksmith = false;

    public bool talkedToAlchemist = false;
    public bool talkedToPriest = false;

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
    [Header("Enemies")]
    public bool isMiniBossKilled = false;

    public bool isBossKilled = false;

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
        isWeaponUpgradeBought = false;
        isHPBought = false;
        isAxeUsed = false;

        swordDamageValue = 1;
        knockbackForce = 1;

        isController = false;
        isKeyboard = false;

        isMiniBossKilled = false;
        isBossKilled = false;

        bookIsLookedAt = false;
        isMoved = false;
        npcInteracted = false;
        combat = false;
        dashed = false;
        door = false;
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