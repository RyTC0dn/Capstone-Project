using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public ItemSO abilityData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory inventory = collision.GetComponent<Inventory>(); 
            if (inventory != null)
            {
                inventory.AddItem(abilityData, collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
