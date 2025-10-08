using UnityEngine;

public class CoinCollection : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        //Check if the player walks into a coin
        if (other.CompareTag("Player") && !other.CompareTag("Weapon"))
        {
            GameManager.instance.coinTracker++;
            if(UIManager.instance != null )
                UIManager.instance.CoinsCollected();
            GameManager.instance.PlayCoinAudio();
            Destroy(gameObject);
        }
    }
}
