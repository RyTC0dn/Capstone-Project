using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public PlayerHealth playerHP { get; private set; }
    private TutorialType tutorial;

    [Header("Health Stats")]
    public int totalHealth = 4;

    [SerializeField] private int currentHealth;
    public bool isInvulnerable = false; //We want to prevent multiple hits on the player
    [SerializeField] private float invulnerableTimer = 2;

    private SpriteRenderer sprite;
    public GameEvent playerHealthChanged;
    public GameEvent playerDeath;

    [Header("Knockback")]
    public float kbForce = 10f;

    public float kbDuration = 0.2f;

    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    private PrototypePlayerMovementControls playerControls;

    private AudioPlayer audioPlayer;
    public AudioSource audioSource;

    public Collider2D playerCollider;

    private void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = GetComponent<PrototypePlayerMovementControls>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioPlayer = GetComponentInChildren<AudioPlayer>();
        currentHealth = totalHealth;
    }

    private void Update()
    {
        //If the player loses all health
        if (currentHealth <= 0)
        {
            if (tutorial == TutorialType.Combat) //Check if the player is in the combat tutorial
            {
                //Just restart the tutorial
                Transform tutorialSpawn = GameManager.instance.zones.playerSpawnPoint.transform;
                GameManager.instance.SendPlayerToStart(this, tutorialSpawn);
            }
            else
            {
                //Otherwise, send player back to town upon losing health
                SendToSTart();
            }
        }
    }

    /// <summary>
    /// Called by GameEventListener when the enemy attack event is raised
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="data"></param>

    public void OnEnemyAttack(Component sender, object data)
    {
        //Validate the data being recieved
        if (!(data is int damage)) return;

        //Ensure sender is a componenet
        if (sender == null) return;
        GameObject sourceObject = sender.gameObject;
        if (sourceObject == null) return;

        //Only accept attacks coming from enemy objects
        //adjust tags where applicable
        bool isEnemySource = sourceObject.CompareTag("GroundEnemy")
            || sourceObject.CompareTag("FlyingEnemy");

        if (!isEnemySource) return;

        TakeDamage(damage, sender);
    }

    //These functions are to define how damage to the player works
    public void TakeDamage(int damageAmount, Component source)
    {
        if (isInvulnerable) { return; }
        StartCoroutine(DamagerRoutine(damageAmount, source));
        //Play player hurt sound effect
        audioPlayer.PlayRandomClip(audioSource, 5, 6);
    }

    private IEnumerator DamagerRoutine(int damageAmount, Component source)
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

    private IEnumerator Knockback(Vector2 direction)
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

    private void SendToSTart()
    {
        SceneManager.LoadScene("Town");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            TakeDamage(1, collision);
        }
    }
}