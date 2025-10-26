using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int totalHealth = 2;
    private int enemyHealth;
    public GameObject coinDrop;

    public GameEvent enemyHealthChanged;

    [Header("Knockback")]
    public float kbForce = 100f;
    public float kbDuration = 0.2f;

    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    BasicEnemyControls enemyControls;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        enemyControls = GetComponent<BasicEnemyControls>();

        enemyHealth = totalHealth;
    }

    private void Update()
    {
        EnemyDeath();
    }

    public void OnPlayerAttack(Component sender, object data)
    {
        if(sender is PlayerWeapon && data is int damage)
        {
            EnemyDamage(damage);
        }
    }

    public void EnemyDamage(int damage)
    {        
       enemyHealth -= damage;
        Debug.Log("Enemy hit");
        enemyHealthChanged.Raise(this, enemyHealth);

        //Knockback function
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            Vector2 direction = (transform.position - player.transform.position).normalized;
            StartCoroutine(Knockback(direction));
        }


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

    IEnumerator Knockback(Vector2 direction)
    {
        isKnockedBack = true;
        enemyControls.enabled = false;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * kbForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(kbDuration);

        rb.linearVelocity = Vector2.zero;
        enemyControls.enabled = true;
        isKnockedBack = false;
    }
}
