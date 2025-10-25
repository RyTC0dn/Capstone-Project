using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int totalHealth = 4;
    private int currentHealth;
    private bool isInvulnerable = false; //We want to prevent multiple hits on the player
    [SerializeField]private float invulnerableTimer = 2;

    private SpriteRenderer sprite;
    public GameEvent playerHealthChanged;

    private void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = totalHealth;
    }

    public void OnEnemyAttack(Component sender, object data)
    {
        //This function will check if the enemy sent out attack event 
        //and if the sent out data was an integer variable
        if(sender is BasicEnemyAttackState && data is int damage)
        {
            TakeDamage(damage);
        }
    }

    //These functions are to define how damage to the player works
    public void TakeDamage(int damageAmount)
    {
        if (isInvulnerable) { return; }
        StartCoroutine(DamagerRoutine(damageAmount));
    }

    IEnumerator DamagerRoutine(int damageAmount)
    {
        currentHealth -= damageAmount; //How much health is lost

        playerHealthChanged.Raise(this, currentHealth);//Raise the player health event after change

        isInvulnerable = true;
        if (isInvulnerable)
        {
            invulnerableTimer--;
            yield return new WaitForSeconds(invulnerableTimer);
            isInvulnerable = false;
        }
        
    }
}
