using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int totalHealth = 2;
    [SerializeField]private int enemyHealth;
    public GameObject coinDrop;
    [SerializeField]private int coinAmount;

    [Header("Knockback")]
    public float kbForce = 100f;
    public float kbDuration = 0.2f;
    public Vector2 knockbackForce;

    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    Rat_Enemy_AI_Logic ratEnemy;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        ratEnemy = GetComponent<Rat_Enemy_AI_Logic>();

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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            float condition = transform.position.x > player.transform.position.x ? 1 : -1;
            StartCoroutine(Knockback(condition));
        }


        EnemyDeath();
    }

    public void EnemyDeath()
    {        
        if (enemyHealth <= 0)
        {
             if(coinAmount < 0)
            {
                Instantiate(coinDrop, transform.position, Quaternion.identity);
            }


            gameObject.SetActive(false);
        }
    }

    #region Knockback Logic
    IEnumerator Knockback(float direction)
    {
        isKnockedBack = true;
        ratEnemy.KnockedBack(true);

        float totalKnockbackForce = GameManager.instance.firstUpgrade ? 
            kbForce * GameManager.instance.currentUpgrade : kbForce;

        //Stop motion
        rb.linearVelocity = Vector2.zero;
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            //Dynamic enemies use physics
            rb.AddForce(knockbackForce * direction, ForceMode2D.Impulse);
        }
        else if (rb.bodyType == RigidbodyType2D.Kinematic) {
            //Kinematic enemies manually move during knockback
            float timer = kbDuration;
            while(timer > 0)
            {
                timer -= Time.deltaTime;
                rb.position += knockbackForce * (direction * 0.02f);
                yield return null;
            }
        }

        yield return new WaitForSeconds(kbDuration);

        //Stop movement again
        rb.linearVelocity = Vector2.zero;

        ratEnemy.KnockedBack(false);
        isKnockedBack = false;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "AbilityPickup")
        {
            //Have the enemy get knocked back but not damaged when colliding with shield
            GameObject shield = GameObject.FindGameObjectWithTag("AbilityPickup");
            Vector2 direction = (transform.position - shield.transform.position).normalized;
        }
    }
}
