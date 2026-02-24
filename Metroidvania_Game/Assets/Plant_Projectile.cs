using UnityEngine;

public class Plant_Projectile : MonoBehaviour
{
    public GameEvent onAttackEvent;
    public GameObject target;
    [SerializeField] private int damage = 1;
    public float projectileSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        //Move towards the player's last known position 
            Vector3 direction = (target.transform.position - transform.position).normalized;
            float speed = projectileSpeed; // Adjust the speed as needed
            transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onAttackEvent.Raise(this, damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
