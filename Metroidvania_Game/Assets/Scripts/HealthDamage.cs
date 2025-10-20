using Unity.VisualScripting;
using UnityEngine;

public class HealthDamage : MonoBehaviour
{
    public int enemyHealth = 2;
    public GameObject coinDrop;

    PrototypePlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = FindAnyObjectByType<PrototypePlayerAttack>();
    }

    private void Update()
    {
        EnemyDeath();
    }

    public void EnemyDamage()
    {        
        enemyHealth-= playerAttack.damageValue;
    }

    public void EnemyDeath()
    {
        if(enemyHealth <= 0)
        {
            Instantiate(coinDrop, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            EnemyDamage();
        }
    }
}
