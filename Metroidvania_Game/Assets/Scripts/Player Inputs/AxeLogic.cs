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
    int damage = 1;

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
            attackEnemyEvent.Raise(this, new AttackDataAxe(collision.gameObject, damage));
            Destroy(gameObject);            
        }

        if (collision.CompareTag("FlyingEnemy"))
        {
            attackEnemyEvent.Raise(this, new AttackDataAxe(collision.gameObject, damage));
            Destroy(gameObject);            
        }

        if (collision.CompareTag("Environment")) { Destroy(gameObject); }

    }

}

public class AttackDataAxe //Just for setting the data that will damage enemies
{
    public GameObject target;
    public int damage;

    public AttackDataAxe(GameObject target, int damage)
    {
        this.target = target;
        this.damage = damage;
    }
}
