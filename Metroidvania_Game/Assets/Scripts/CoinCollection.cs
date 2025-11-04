using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    public int coinsCollected;
    private bool playerDetected = false;
    public GameEvent coinCollection;
    private int coinValue = 1;

    private void CoinCollect(int amount)
    {
        coinsCollected += amount;
        coinCollection.Raise(this, amount);      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the player walks into a coin
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Coin Collected");

            CoinCollect(coinValue);

            Destroy(gameObject);
        }
    }
}
