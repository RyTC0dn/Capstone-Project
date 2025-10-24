using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int totalHealth = 4;
    private int currentHealth;
    private bool isInvulnerable = false; //We want to prevent multiple hits on the player
    private float invulnerableTimer = 2;

    private SpriteRenderer sprite;
    public GameEvent playerHealthChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = totalHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if(isInvulnerable) { return; }
        StartCoroutine(DamagerRoutine(damageAmount));
    }

    IEnumerator DamagerRoutine(int damageAmount)
    {
        currentHealth -= damageAmount;

        playerHealthChanged.Raise(this, currentHealth);

        isInvulnerable = true;

        if(sprite != null)
        {
            float flashTime = 0.5f; 
            for(float i = 0; i < invulnerableTimer; i++)
            {
                sprite.enabled = false;
                yield return new WaitForSeconds(flashTime);
                sprite.enabled = true;
                yield return new WaitForSeconds(flashTime);
            }
        }
        else
        {
            yield return new WaitForSeconds(invulnerableTimer);
        }

        isInvulnerable = false;
    }
}
