using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(BoxCollider2D))]
public class DebrisCollision : MonoBehaviour
{
    [SerializeField] private float debrisLifetime; // Lifetime of debris in seconds
    [SerializeField]private int damage;
    [SerializeField]private GameEvent damagePlayer;
    private ParticleSystem rockExplosion;

    private void Start()
    {
        // Schedule debris destruction after its lifetime
        Destroy(gameObject, debrisLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject == null) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (damagePlayer == null)
            {
                Debug.LogWarning("Damage Player GameEvent is not assigned in DebrisCollision script.");
                return;
            }

            // Deal damage to the player
            damagePlayer.Raise(this, damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        // Start the coroutine to destroy debris after its lifetime
        StartCoroutine(DestroyDebrisAfterTime());

    }

    private System.Collections.IEnumerator DestroyDebrisAfterTime()
    {
        yield return new WaitForSeconds(debrisLifetime);
        Destroy(gameObject);
    }
}
