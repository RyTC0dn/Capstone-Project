using UnityEngine;

public class SpikeLogic : MonoBehaviour
{

    [SerializeField] int damage = 1;
    PrototypePlayerMovementControls playerControls;


    private void Start()
    {
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.PlayerDamaged(damage);
            playerControls.gameObject.transform.position = GameManager.instance.playerSpawnPoint.transform.position;
        }

    }
}
