using UnityEngine;

public class SpikeLogic : MonoBehaviour
{

    [SerializeField] int damage = 1;
    PrototypePlayerMovementControls playerControls;

    public GameEvent onDamaged; //Call an event that will entity health


    private void Start()
    {
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Add player health change
            onDamaged.Raise(this, damage);
        }

    }
}
