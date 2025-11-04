using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int damageValue = 1;

    [Header("Game Event")]
    public GameEvent onPlayerAttack;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundEnemy") || collision.CompareTag("FlyingEnemy"))
        {
            onPlayerAttack.Raise(this, new AttackData(collision.gameObject, damageValue));
        }
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
