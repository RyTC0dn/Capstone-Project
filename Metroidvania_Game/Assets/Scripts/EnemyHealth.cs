using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int totalHealth = 2;
    private int enemyHealth;
    public GameObject coinDrop;

    public GameEvent enemyHealthChanged;

    private void Start()
    {
        enemyHealth = totalHealth;
    }

    private void Update()
    {
        EnemyDeath();
    }

    public void EnemyDamage(int damage)
    {        
       enemyHealth -= damage;

        enemyHealthChanged.Raise(this, enemyHealth);
        EnemyDeath();
    }

    public void EnemyDeath()
    {
        if(enemyHealth <= 0)
        {
            Instantiate(coinDrop, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
