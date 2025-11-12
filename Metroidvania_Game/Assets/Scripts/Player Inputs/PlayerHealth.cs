using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Stats")]
    public int totalHealth = 4;
    [SerializeField]private int currentHealth;
    public bool isInvulnerable = false; //We want to prevent multiple hits on the player
    [SerializeField]private float invulnerableTimer = 2;

    private SpriteRenderer sprite;
    public GameEvent playerHealthChanged;
    public GameEvent playerDeath;

    [Header("Knockback")]
    public float kbForce = 10f;
    public float kbDuration = 0.2f;

    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    PrototypePlayerMovementControls playerControls;

    private void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = GetComponent<PrototypePlayerMovementControls>();
        sprite = GetComponent<SpriteRenderer>();    
        currentHealth = totalHealth;
    }

    private void Update()
    {
        if(currentHealth <= 0)
        {
            SendToSTart();
        }
    }

    /// <summary>
    /// Called by GameEventListener when the enemy attack event is raised
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="data"></param>

    public void OnEnemyAttack(Component sender, object data)
    {
        //This function will check if the enemy sent out attack event 
        //and if the sent out data was an integer variable
        if(data is int damage)
        {
            TakeDamage(damage, sender);
        }
    }

    //These functions are to define how damage to the player works
    public void TakeDamage(int damageAmount, Component source = null)
    {
        if (isInvulnerable) { return; }
        StartCoroutine(DamagerRoutine(damageAmount, source));

    }

    IEnumerator DamagerRoutine(int damageAmount, Component source)
    {
        currentHealth -= damageAmount; //How much health is lost

        //Notify UI on health change
        playerHealthChanged.Raise(this, currentHealth);//Raise the player health event after change

        //Invulnerability function
        isInvulnerable = true;

        //Death & send to start function
        if (currentHealth == 2)
        {
            Transform newPos = transform;
            playerHealthChanged.Raise(this, newPos);
        }
        else if (currentHealth <= 0)
        {
            Transform deathPos = transform;
            playerDeath.Raise(this, deathPos);
        }

        //Knockback function
        Vector2 direction = (transform.position - source.transform.position).normalized;
        StartCoroutine(Knockback(direction));

        if (sprite != null)
        {
            Color color = sprite.color;
            float elapsed = 0f;

            while (elapsed < invulnerableTimer)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.PingPong(Time.time * 4f, 0.5f) + 0.5f; // oscillates between 0.5–1
                sprite.color = new Color(color.r, color.g, color.b, alpha); //Flashing transparency
                yield return null;
            }

            sprite.color = new Color(color.r, color.g, color.b, 1f);
        }
        else
        {
            yield return new WaitForSeconds(invulnerableTimer);
        }
        isInvulnerable = false;
    }

    IEnumerator Knockback(Vector2 direction)
    {
        isKnockedBack = true;
        playerControls.enabled = false;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * kbForce, ForceMode2D.Impulse);
        Debug.Log("Player knocked back");

        yield return new WaitForSeconds(kbDuration);

        rb.linearVelocity = Vector2.zero;
        playerControls.enabled = true;
        isKnockedBack = false;
    }

    void SendToSTart()
    {
        SceneManager.LoadScene("Town");
    }
}
