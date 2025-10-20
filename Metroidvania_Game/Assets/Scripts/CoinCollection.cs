using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    public int coinValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the player walks into a coin
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Coin Collected");
            GameManager.instance.CoinCollection(coinValue);
            Destroy(gameObject);
        }
    }
}
