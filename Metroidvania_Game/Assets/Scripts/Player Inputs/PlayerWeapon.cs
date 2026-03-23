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
    public float damageValue = 1;

    public float knockbackForce = 1;

    public int damagePerUpgrade = 1;

    [SerializeField] private float currentDamage;
    [SerializeField] private float currentKnockback;

    [Header("Game Event")]
    public GameEvent onPlayerAttack;

    public GameEvent onKnockback;

    public SceneInfo sceneInfo;

    private bool hasBeenUpgraded = false;

    private void Start()
    {
        currentDamage = damageValue * sceneInfo.swordDamageValue;
        currentKnockback = knockbackForce * sceneInfo.knockbackForce;
    }

    private void Update()
    {
        //currentDamage = DamageHandler();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundEnemy") || collision.CompareTag("FlyingEnemy"))
        {
            onPlayerAttack.Raise(this, new AttackData(collision.gameObject, currentDamage));
            onPlayerAttack.Raise(this, new KnockbackData(collision.gameObject, currentKnockback));
        }
    }

    //private int DamageHandler()
    //{
    //    //if (sceneInfo.isWeaponUpgradeBought)
    //    //{
    //    //    int damageCalc = currentDamage + sceneInfo.swordDamageValue;
    //    //    Debug.Log(damageCalc);
    //    //    sceneInfo.isWeaponUpgradeBought = false;
    //    //    return damageCalc;
    //    //}
    //    //return currentDamage;
    //}
}

/// <summary>
/// Represents information about an attack, including the target and the amount of damage dealt.
/// </summary>
/// <remarks>Use this class to encapsulate attack details when processing combat actions or events. The target
/// specifies the recipient of the attack, and the damage indicates the amount inflicted. This class is typically used
/// in game logic to pass attack data between systems.</remarks>
public class AttackData
{
    public GameObject target;
    public float damage;

    public AttackData(GameObject target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }
}

/// <summary>
/// Represents knockback information for a specific game object, including the target and the magnitude of the knockback
/// effect.
/// </summary>
/// <remarks>Use this class to encapsulate knockback data when applying force or effects to game objects in
/// gameplay scenarios. The target field identifies the affected object, while knockback specifies the strength of the
/// effect.</remarks>
public class KnockbackData
{
    public GameObject target;
    public float knockback;

    public KnockbackData(GameObject target, float knockback)
    {
        this.target = target;
        this.knockback = knockback;
    }
}

public class StompData
{
    public GameObject target;
    public float damage;

    public StompData(GameObject target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }
}