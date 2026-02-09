using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    public int coinsCollected;
    private bool playerDetected = false;
    public GameEvent coinCollection;
    private int coinValue = 1;
    private AudioSource coinCollect;

    [Header("Coin Pulling Values")]
    [SerializeField]private float pullRadius = 5f; // Adjust this value to control the pull radius
    [SerializeField]private float pullSpeed = 5f; // Adjust this value to control the pull speed

    private void CoinCollect(int amount)
    {
        coinsCollected += amount;
        coinCollection.Raise(this, amount);      
    }


    private void Update()
    {
        CoinPull();
    }

    private void CoinPull()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            if(Vector2.Distance(transform.position, player.transform.position) <= pullRadius)
            {
                playerDetected = true;
            }
            else
            {
                playerDetected = false;
            }
            if(playerDetected)
            {
                Vector3 direction = player.transform.position - transform.position;
                float pullStrength = pullSpeed; // Adjust this value to control the pull strength
                transform.position += direction.normalized * pullStrength * Time.deltaTime;
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        coinCollect = GetComponent<AudioSource>();

        //Check if the player walks into a coin
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Coin Collected");

            CoinCollect(coinValue);

            coinCollect.Play();

            Destroy(gameObject);
        }
    }
}
