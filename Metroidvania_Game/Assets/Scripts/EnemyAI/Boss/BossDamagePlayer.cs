using UnityEngine;

public class BossDamagePlayer : MonoBehaviour
{
    public int damage = 1;
    public GameEvent playerHealthChanged;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerHealthChanged.Raise(this, damage);
            Debug.Log("Player hit by boss attack, damage: " + damage);
        }
    }
}