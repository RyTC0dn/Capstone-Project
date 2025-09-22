using UnityEngine;

public class BasicEnemyHealth : MonoBehaviour
{
    public int enemyHealth = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDeath()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            enemyHealth--;
            Debug.Log("Enemy hit!!!");
        }
    }
}
