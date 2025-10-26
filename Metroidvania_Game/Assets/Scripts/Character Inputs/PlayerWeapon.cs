using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int damageValue = 1;

    [Header("Game Event")]
    public GameEvent onPlayerAttack;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundEnemy"))
        {
            onPlayerAttack.Raise(this, damageValue);
        }
    }
}
