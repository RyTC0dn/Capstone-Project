using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public float bulletSpeed = 1.0f;
    public int bulletDamage = 1;
    private Rigidbody2D rb2D;
    private float activeTimer = 10f;
    private bool hasHit = false;

    public GameEvent onPlayerHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        Destroy(gameObject, activeTimer);
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Player"))
        {
            //Raise event to damage player
            onPlayerHit.Raise(this, bulletDamage);
            Destroy(gameObject);
        }
    }

}
