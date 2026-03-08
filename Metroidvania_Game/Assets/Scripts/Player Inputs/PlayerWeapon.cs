using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Represents a player's weapon in the game, managing its damage value, upgrade state, and attack events.
/// </summary>
/// <remarks>PlayerWeapon handles weapon statistics, upgrade effects, and triggers attack-related game events when
/// interacting with enemies. Attach this component to a player character to enable weapon functionality and
/// event-driven attack handling.</remarks>
public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int damageValue = 1;

    public int damagePerUpgrade = 1;

    [SerializeField] private int currentDamage;

    [Header("Game Event")]
    public GameEvent onPlayerAttack;

    public SceneInfo sceneInfo;

    private bool hasBeenUpgraded = false;

    private void Start()
    {
        currentDamage = damageValue;
    }

    private void Update()
    {
        //currentDamage = DamageHandler();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundEnemy") || collision.CompareTag("FlyingEnemy"))
        {
            onPlayerAttack.Raise(this, new AttackData(collision.gameObject, sceneInfo.swordDamageValue));
        }
    }

    private int DamageHandler()
    {
        if (sceneInfo.isWeaponUpgradeBought)
        {
            int damageCalc = currentDamage + sceneInfo.swordDamageValue;
            Debug.Log(damageCalc);
            sceneInfo.isWeaponUpgradeBought = false;
            return damageCalc;
        }
        return currentDamage;
    }
}

public class AttackData
{
    public GameObject target;
    public int damage;

    public AttackData(GameObject target, int damage)
    {
        this.target = target;
        this.damage = damage;
    }
}