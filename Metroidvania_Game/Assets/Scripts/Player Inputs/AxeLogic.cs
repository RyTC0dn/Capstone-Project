using UnityEngine;

public class AxeLogic : MonoBehaviour
{
    PrototypePlayerMovementControls playerControls;

    [SerializeField]
    float vSpeed = 1.0f;
    [SerializeField]
    float hSpeed = 1.0f;
    [SerializeField]
    float attackRate = 1.0f;
    [SerializeField]
    float damage = 1.0f;

    public GameEvent attackEnemyEvent;


    private float nextTimeToAttack;

    [SerializeField]
    Rigidbody2D rb2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();

        rb2D.linearVelocity = (transform.right * vSpeed) + (transform.up * hSpeed);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundEnemy"))
        {
            attackEnemyEvent.Raise(this, damage);
            Destroy(gameObject);            
        }

        if (collision.CompareTag("FlyingEnemy"))
        {
            attackEnemyEvent.Raise(this, damage);
            Destroy(gameObject);            
        }

        if (collision.CompareTag("Environment")) { Destroy(gameObject); }

    }

}
