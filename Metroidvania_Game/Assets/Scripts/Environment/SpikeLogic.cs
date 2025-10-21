using UnityEngine;

public class SpikeLogic : MonoBehaviour
{

    [SerializeField] int damage = 1;
    PrototypePlayerMovementControls playerControls;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.PlayerDamaged(damage);
            playerControls.gameObject.transform.position = playerControls.playerSpawnPoint.position;
        }

    }
}
