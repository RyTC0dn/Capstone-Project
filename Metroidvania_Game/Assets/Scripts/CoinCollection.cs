using System.Collections;
using UnityEngine;

public enum CoinType
{
    Level,
    Treasure
}

public class CoinCollection : MonoBehaviour
{
    public static CoinCollection coin { get; private set; }
    public int coinsCollected;
    private bool playerDetected = false;
    public GameEvent coinCollection;
    private int coinValue = 1;
    private AudioSource coinCollect;
    public CoinType coinType;

    [Header("Coin Pulling Values")]
    [SerializeField] private float pullRadius = 5f; // Adjust this value to control the pull radius

    [SerializeField] private float pullSpeed = 5f; // Adjust this value to control the pull speed

    [Header("Coin Fountain")]
    [Tooltip("Only applies if the coin type is Treasure")]
    [SerializeField] private float throwForce = 5f; // Adjust this value to control the throw force

    [SerializeField] private float timeInAir; // Adjust this value to control the time the coin stays in the air after being thrown
    [SerializeField] private float delayTillDelete = 0.5f; // Adjust this value to control the delay before the coin is destroyed after being collected

    private Rigidbody2D rb;
    private bool isThrown = false;
    public SpriteRenderer spriteRenderer;
    public Collider2D coinCollider;
    public AudioPlayer player;
    public bool overlapAudio = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coinCollect = GetComponentInChildren<AudioSource>();

        SetCoinType(this.coinType);
    }

    private void CoinCollect(int amount)
    {
        coinsCollected += amount;
        coinCollection.Raise(this, amount);
    }

    private void Update()
    {
        // Only pull coins if they are not thrown from a treasure chest
        CoinPull();
    }

    private void CoinPull()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= pullRadius)
            {
                playerDetected = true;
            }
            else
            {
                playerDetected = false;
            }
            if (playerDetected)
            {
                Vector3 direction = player.transform.position - transform.position;
                float pullStrength = pullSpeed; // Adjust this value to control the pull strength
                transform.position += direction.normalized * pullStrength * Time.deltaTime;
            }
        }
    }

    public void SetCoinType(CoinType type)
    {
        switch (type)
        {
            case CoinType.Level:
                rb.bodyType = RigidbodyType2D.Kinematic;
                coinCollider.isTrigger = true;
                break;

            case CoinType.Treasure:
                LaunchCoin();
                coinCollider.isTrigger = false;
                break;

            default:
                break;
        }
    }

    public void AudioTrigger(CoinType type)
    {
        switch (type)
        {
            case CoinType.Level:
                player.PlayAudio(0, coinCollect, overlapAudio); // Play the coin collection sound effect
                break;

            case CoinType.Treasure:
                player.PlayAudio(1, coinCollect, overlapAudio); // Play the coin collection sound effect
                break;

            default:
                break;
        }
    }

    private void LaunchCoin()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the coin.");
            return;
        }

        rb.bodyType = RigidbodyType2D.Dynamic; // Ensure the coin is dynamic for physics interactions
        isThrown = true;

        float angle = Random.Range(45, 90); // Randomize the launch angle between 45 and 90 degrees
        float rad = angle * Mathf.Deg2Rad; // Convert angle to radians

        Vector2 launchDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized; // Randomize the launch direction

        // Apply an impulse force to the coin in the launch direction
        rb.AddForce(launchDir * throwForce, ForceMode2D.Impulse);
    }

    // Handle collision with the player or ground
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the player walks into a coin
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("AbilityPickup"))
        {
            Debug.Log("Coin Collected");

            CoinCollect(coinValue);

            spriteRenderer.enabled = false; // Hide the coin's sprite immediately upon collection
            AudioTrigger(CoinType.Level); // Play the coin collection sound effect
            Destroy(gameObject, delayTillDelete);
        }
    }

    // This method is used for coins that are thrown from treasure chests,
    // allowing them to be collected upon collision with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if the player walks into a coin
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("AbilityPickup"))
        {
            Debug.Log("Coin Collected");

            CoinCollect(coinValue);

            spriteRenderer.enabled = false; // Hide the coin's sprite immediately upon collection
            AudioTrigger(CoinType.Level); // Play the coin collection sound effect
            Destroy(gameObject, delayTillDelete);
        }
        else if (collision.gameObject.CompareTag("Environment"))
        {
            AudioTrigger(CoinType.Treasure);
            SetCoinType(CoinType.Level); // Change the coin type to Level after it hits the ground

            //Should also stop their movement once they hit the ground
            Vector2 stopForce = Vector2.zero;
            rb.AddForce(stopForce);
        }
    }
}