using UnityEngine;

/// <summary>
/// Represents a projectile fired by the plant boss that moves toward the player's last known position and triggers an attack
/// event upon collision.
/// </summary>
/// <remarks>This component should be attached to a projectile GameObject in a Unity scene. The projectile
/// automatically seeks the player at the time of instantiation and is destroyed either upon collision with the player
/// or ground, or when its lifetime expires. The attack event is raised only when the projectile collides with the
/// player. Configure the projectile's speed, damage, and lifetime in the Unity Inspector as needed.</remarks>
public class Plant_Projectile : MonoBehaviour
{
    public GameEvent onAttackEvent;
    private Rigidbody2D rb2D;
    private Quaternion rotation;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime;
    public float projectileSpeed = 5f;

    private SpitFirepoint firepoint;

    private void Awake()
    {
        firepoint = FindFirstObjectByType<SpitFirepoint>();
        transform.rotation = firepoint.transform.rotation;
        Debug.Log($"Move in {firepoint.name}");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    private void Update()
    {
        // Continuously move towards the player's last known position
        //MoveTowardsPlayer(lastKnownPosition);
        transform.Translate(Vector2.right * projectileSpeed * Time.deltaTime);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)//Destroy the projectile after its lifetime expires
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collision logic: if the projectile collides with the player, raise the attack event and destroy the projectile.
        //If it collides with the ground, just destroy the projectile.
        if (collision.gameObject.CompareTag("Player"))
        {
            onAttackEvent.Raise(this, damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Environment"))
        {
            //Optionally, could add some animation and/or sound effect here before destroying the projectile
            Destroy(gameObject);
        }
    }
}