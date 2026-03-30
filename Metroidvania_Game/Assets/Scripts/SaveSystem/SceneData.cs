using UnityEngine;

[System.Serializable]
public class SceneData
{
    public bool shieldPickupData;
    public bool wallBreakPickupData;
    public bool shieldUsedData;
    public bool wallBreakUsedData;

    public bool axeBoughtData;
    public bool weaponUpgradeData;
    public bool hpBoughtData;
    public bool axeUsedData;
    public float swordDamageData;
    public float knockbackData;
    public bool talkedToBlacksmithData;
    public bool talkedToAlchemistData;
    public bool talkedToPriestData;

    public bool bossCutscenePlayedData;

    public bool bookIsLookedAtData;

    public bool isMovedData;
    public bool npcInteractedData;
    public bool combatData;
    public bool dashedData;
    public bool doorData;

    public bool miniBossDeadData;

    public SceneData (SceneInfo sceneInfo)
    {
        shieldPickupData = sceneInfo.isShieldPickedUp;
        wallBreakPickupData = sceneInfo.isWallBreakPickedUp;
        shieldUsedData = sceneInfo.isShieldUsed;
        wallBreakUsedData = sceneInfo.isWallBreakUsed;

        axeBoughtData = sceneInfo.isAxeBought;
        weaponUpgradeData = sceneInfo.isWeaponUpgradeBought;
        hpBoughtData = sceneInfo.isHPBought;
        axeBoughtData = sceneInfo.isAxeUsed;
        swordDamageData = sceneInfo.swordDamageValue;
        knockbackData = sceneInfo.knockbackForce;
        talkedToBlacksmithData = sceneInfo.talkedToBlacksmith;
        talkedToAlchemistData = sceneInfo.talkedToAlchemist;
        talkedToPriestData = sceneInfo.talkedToPriest;

        bossCutscenePlayedData = sceneInfo.isBossCutscenePlayed;

        bookIsLookedAtData = sceneInfo.bookIsLookedAt;

        isMovedData = sceneInfo.isMoved;
        npcInteractedData = sceneInfo.npcInteracted;
        combatData = sceneInfo.combat;
        dashedData = sceneInfo.dashed;
        doorData = sceneInfo.door;

        miniBossDeadData = sceneInfo.isMiniBossKilled;

    }

}
