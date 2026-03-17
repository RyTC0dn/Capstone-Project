using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Represents the health and death behavior of an enemy character in the game, including handling damage, knockback,
/// and coin drop upon defeat.
/// </summary>
/// <remarks>Attach this component to enemy GameObjects to manage their health state and respond to player attacks
/// or environmental hazards. The class interacts with other components such as Rigidbody2D, BasicEnemyControls,
/// Knockback, and SceneInfo to provide knockback effects and coordinate enemy death events. Coin drops are instantiated
/// at the enemy's position when defeated. Ensure required components are assigned in the inspector for proper
/// functionality.</remarks>

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float totalHealth = 2;

    [SerializeField] private float enemyHealth;
    public GameObject coinDrop;

    [Header("Knockback")]
    private Rigidbody2D rb;

    public SceneInfo sceneInfo; //Assign in inspector

    private bool isKnockedBack = false;
    private SpriteRenderer sp;

    private BasicEnemyControls enemyControls;

    private Knockback kb;

    [Header("Hit flash setting")]
    [SerializeField] private float flashTime = 0.5f;

    [SerializeField] private AudioSource enemyAudioScorce;
    [SerializeField] private AudioClip enemyHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        enemyControls = GetComponent<BasicEnemyControls>();

        sp = GetComponent<SpriteRenderer>();

        kb = GetComponent<Knockback>();

        enemyAudioScorce = GetComponent<AudioSource>();

        enemyHealth = totalHealth;
    }

    private void Update()
    {
        EnemyDeath();
    }

    /// <summary>
    /// Handles attack events directed at the enemy, applying damage based on the source and attack data.
    /// </summary>
    /// <remarks>This method processes both direct player attacks and collisions with debris. Damage is
    /// applied only if the attack targets this enemy or if debris collides with it. Ensure that the data parameter
    /// matches the expected type for the sender to avoid ignored events.</remarks>
    /// <param name="sender">The component that initiated the attack event. Can represent a player, debris, or other sources.</param>
    /// <param name="data">The event data associated with the attack. Can be an AttackData object containing attack details or an integer
    /// representing debris damage.</param>
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

    /// <summary>
    /// Applies damage to the enemy and triggers knockback and visual feedback effects.
    /// </summary>
    /// <remarks>If the player object is present in the scene, the enemy will be knocked back and a visual
    /// flash effect will occur. This method also checks for enemy death after applying damage.</remarks>
    /// <param name="damage">The amount of damage to subtract from the enemy's health. Must be a non-negative integer.</param>
    public void EnemyDamage(float damage)
    {
        //Deplete health points
        enemyHealth -= damage;
        enemyAudioScorce.clip = enemyHit;
        enemyAudioScorce.Play();
        Debug.Log("Enemy hit");

        //Knockback function
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 direction = (transform.position - player.transform.position).normalized;

            kb.CallKnockback(direction, Vector2.up, enemyControls.enemySpeed * sceneInfo.knockbackForce);
            StartCoroutine(FlashSprite());
        }

        EnemyDeath();
    }

    public void EnemyDeath()
    {
        if (enemyHealth <= 0)
        {
            Instantiate(coinDrop, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Animates the sprite by gradually flashing it red during the knockback period, then restores its original color.
    /// </summary>
    /// <remarks>This coroutine is intended to be used with Unity's StartCoroutine method. The sprite is set
    /// to red and its opacity increases over the knockback time, after which it returns to white. If the sprite
    /// renderer is not assigned, the animation is skipped.</remarks>
    /// <returns>An enumerator that performs the sprite flash animation over the knockback duration.</returns>
    private IEnumerator FlashSprite()
    {
        if (sp != null)
        {
            Color color = Color.red;
            float elapsed = 0f;

            while (elapsed < flashTime)
            {
                elapsed += Time.deltaTime;
                //float flash = Mathf.Clamp01(elapsed / flashTime);
                float flashing = Mathf.PingPong(Time.time * 4f, 0.5f) + 0.5f;
                sp.color = new Color(color.r, color.g, color.b, flashing);
                yield return null;
            }
            //Return to white base after
            sp.color = Color.white;
        }

        yield return new WaitForSeconds((int)kb.knockbackTime);
    }

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