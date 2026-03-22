using UnityEngine;

public class StompHIt : MonoBehaviour
{
    [SerializeField] private float bounce;
    [SerializeField] private Rigidbody2D playerRB2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundEnemy") || collision.CompareTag("FlyingEnemy"))
        {


            playerRB2D.linearVelocity = new Vector2(playerRB2D.linearVelocityX, bounce);
        }
    }
}
