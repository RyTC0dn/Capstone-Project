using UnityEngine;
using UnityEngine.Rendering;

public class TownEvents : MonoBehaviour
{
    public Collider2D destination;
    public float offset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 position = destination.transform.position;
            position.x += offset;
            collision.gameObject.transform.position = position;
        }
    }
}
