using UnityEngine;

public class SpikeLogic : MonoBehaviour
{

    [SerializeField] int damage = 1;
    PrototypePlayerMovementControls playerControls;

    public GameEvent onDamaged; //Call an event that will entity health

    PlayerHealth playerHP;


    private void Start()
    {
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();
        playerHP = FindFirstObjectByType<PlayerHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Add player health change
            playerHP.TakeDamage(damage, this);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Add player health change
            playerHP.TakeDamage(damage, this);
        }
    }
}
