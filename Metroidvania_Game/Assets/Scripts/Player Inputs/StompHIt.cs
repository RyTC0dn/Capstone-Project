using UnityEngine;

public class StompHIt : MonoBehaviour
{
    [SerializeField] private float bounce;
    [SerializeField] private Rigidbody2D playerRB2D;

    public GameEvent onPlayerStomp;
    [SerializeField] private float stompDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundEnemy"))
        {
            onPlayerStomp.Raise(this, new StompData(collision.gameObject, stompDamage));

            playerRB2D.linearVelocity = new Vector2(playerRB2D.linearVelocityX, bounce);
        }
    }
}
