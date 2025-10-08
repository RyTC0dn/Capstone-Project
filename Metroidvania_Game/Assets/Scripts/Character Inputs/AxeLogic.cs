using UnityEngine;

public class AxeLogic : MonoBehaviour
{
    PrototypePlayerMovementControls playerControls;

    [SerializeField]
    float vSpeed = 1.0f;
    [SerializeField]
    float hSpeed = 1.0f;
    [SerializeField]
    Rigidbody2D rb2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();

        if (playerControls.isFacingRight)
        {
            rb2D.linearVelocity = (transform.right * vSpeed) + (transform.up * hSpeed);
        }
        else if (!playerControls.isFacingRight)
        {
            rb2D.linearVelocity = (transform.right * -vSpeed) + (transform.up * hSpeed);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundEnemy"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("FlyingEnemy"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Environment")) { Destroy(gameObject); }

    }

}
