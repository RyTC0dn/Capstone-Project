using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int totalHealth = 2;

    [SerializeField] private int enemyHealth;
    public GameObject coinDrop;

    [Header("Knockback")]
    private Rigidbody2D rb;

    private bool isKnockedBack = false;
    private SpriteRenderer sp;

    private BasicEnemyControls enemyControls;

    private Knockback kb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        enemyControls = GetComponent<BasicEnemyControls>();

        sp = GetComponent<SpriteRenderer>();

        kb = GetComponent<Knockback>();

        enemyHealth = totalHealth;
    }

    private void Update()
    {
        EnemyDeath();
    }

    public void OnPlayerAttack(Component sender, object data)
    {
        if (data is AttackData attack)
        {
            if (attack.target == this.gameObject)
            {
                EnemyDamage(attack.damage);
                Debug.Log("Recieved attack");
            }
        }
        if (sender is DebrisCollision debris && data is int debrisDamage)
        {
            EnemyDamage(debrisDamage);
            Debug.Log("Hit by debris");
        }
    }

    public void OnAxeAttack(Component sender, object data)
    {
        if (data is AttackDataAxe axe)
        {
            if (axe.target == this.gameObject)
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
        if (player != null)
        {
            Vector2 direction = (transform.position - player.transform.position).normalized;

            kb.CallKnockback(direction, Vector2.up, enemyControls.enemySpeed);
            StartCoroutine(FlashSprite());
            //StartCoroutine(Knockback(direction));
        }

        EnemyDeath();
    }

    public void EnemyDeath()
    {
        if (enemyHealth <= 0)
        {
            Instantiate(coinDrop, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    #region Knockback Logic

    //private IEnumerator Knockback(Vector2 direction)
    //{
    //    isKnockedBack = true;
    //    enemyControls.enabled = false;

    //    float totalKnockbackForce = GameManager.instance.firstUpgrade ?
    //        kbForce * GameManager.instance.currentUpgrade : kbForce;

    //    StartCoroutine(FlashSprite());

    //    //Stop motion
    //    rb.linearVelocity = Vector2.zero;
    //    if (rb.bodyType == RigidbodyType2D.Dynamic)
    //    {
    //        //Dynamic enemies use physics
    //        rb.AddForce(direction * totalKnockbackForce, ForceMode2D.Impulse);
    //    }
    //    else if (rb.bodyType == RigidbodyType2D.Kinematic)
    //    {
    //        //Kinematic enemies manually move during knockback
    //        float timer = kbDuration;
    //        while (timer > 0)
    //        {
    //            timer -= Time.deltaTime;
    //            rb.position += direction * (totalKnockbackForce * 0.02f);
    //            yield return null;
    //        }
    //    }

    //    yield return new WaitForSeconds(kbDuration);

    //    //Stop movement again
    //    rb.linearVelocity = Vector2.zero;

    //    enemyControls.enabled = true;
    //    isKnockedBack = false;
    //}

    private IEnumerator FlashSprite()
    {
        if (sp != null)
        {
            Color color = Color.red;
            float elapsed = 0f;

            while (elapsed < kb.knockbackTime)
            {
                elapsed += Time.deltaTime;
                float flash = Mathf.Clamp01(elapsed / kb.knockbackTime);
                sp.color = new Color(color.r, color.g, color.b, flash);
                yield return null;
            }
            //Return to white base after
            sp.color = Color.white;
        }

        yield return new WaitForSeconds((int)kb.knockbackTime);
    }

    #endregion Knockback Logic

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "AbilityPickup")
        {
            //Have the enemy get knocked back but not damaged when colliding with shield
            GameObject shield = GameObject.FindGameObjectWithTag("AbilityPickup");
            Vector2 direction = (transform.position - shield.transform.position).normalized;
            //StartCoroutine(Knockback(direction));
        }
    }
}