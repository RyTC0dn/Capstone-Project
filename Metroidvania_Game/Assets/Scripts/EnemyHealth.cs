using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int totalHealth = 2;
    [SerializeField]private int enemyHealth;
    public GameObject coinDrop;

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
        if(data is AttackData attack)
        {
            if (attack.target == this.gameObject)
            {
                EnemyDamage(attack.damage);
                Debug.Log("Recieved attack");
            }
        }
    }

    public void OnAxeAttack(Component sender, object data)
    {
        if (data is AttackDataAxe axe)
        {
            if(axe.target == this.gameObject)
            {
                EnemyDamage(axe.damage);
                Debug.Log("Axe hit!");
            }
        }
    }

    public void EnemyDamage(int damage)
    {        
       enemyHealth -= damage;
        Debug.Log("Enemy hit");

        //Knockback function
        GameObject player = GameObject.FindGameObjectWithTag("Weapon");
        if(player != null)
        {
            Vector2 direction = (transform.position - player.transform.position).normalized;
            StartCoroutine(Knockback(direction));
        }


        EnemyDeath();
    }

    public void EnemyDeath()
    {        
        if (enemyHealth <= 0)
        {
            Instantiate(coinDrop, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            Destroy(gameObject, 1);
        }
    }


    IEnumerator Knockback(Vector2 direction)
    {
        isKnockedBack = true;
        enemyControls.enabled = false;

        float totalKnockbackForce = GameManager.instance.firstUpgrade ? 
            kbForce * GameManager.instance.currentUpgrade : kbForce;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * totalKnockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(kbDuration);

        rb.linearVelocity = Vector2.zero;
        enemyControls.enabled = true;
        isKnockedBack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "AbilityPickup")
        {
            //Have the enemy get knocked back but not damaged when colliding with shield
            GameObject shield = GameObject.FindGameObjectWithTag("AbilityPickup");
            Vector2 direction = (transform.position - shield.transform.position).normalized;
            StartCoroutine(Knockback(direction));
        }
    }
}
