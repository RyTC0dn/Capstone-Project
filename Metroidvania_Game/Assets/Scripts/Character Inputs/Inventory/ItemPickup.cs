using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameEvent shieldPickup;
    private bool hasPickedUp = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && gameObject.tag == "Shield")
        {
            hasPickedUp = true;
            shieldPickup.Raise(this, hasPickedUp);
            Destroy(gameObject);
        }
    }
}
