using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    public int coinsCollected;
    private bool playerDetected = false;
    public GameEvent coinCollection;

    private void CoinCollect(int amount)
    {
        if (playerDetected)
        {
            coinsCollected += amount;
            coinCollection.Raise(this, amount);
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the player walks into a coin
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Coin Collected");
            playerDetected = true;

            CoinCollect(1);

            Destroy(gameObject);
        }
    }
}
