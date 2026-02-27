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
    private GameObject target;
    private Vector3 lastKnownPosition;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime;
    public float projectileSpeed = 5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Find the player and set the last known position to the player's current position
        target = GameObject.FindGameObjectWithTag("Player");
        lastKnownPosition = target.transform.position;
    }

    private void Update()
    {
        // Continuously move towards the player's last known position
        MoveTowardsPlayer(lastKnownPosition);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)//Destroy the projectile after its lifetime expires
        {
            Destroy(gameObject);
        }
    }

    private void MoveTowardsPlayer(Vector3 lastPos)
    {
        //Move towards the player's last known position 
            Vector3 direction = (lastPos - transform.position).normalized;
            float speed = projectileSpeed; // Adjust the speed as needed
            transform.position += direction * speed * Time.deltaTime;

        if(transform.position == lastPos)
        {
            //Optionally, could add some animation and/or sound effect here before destroying the projectile
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
        else if (collision.gameObject.CompareTag("Ground"))
        {
            //Optionally, could add some animation and/or sound effect here before destroying the projectile
            Destroy(gameObject);
        }
    }
}
