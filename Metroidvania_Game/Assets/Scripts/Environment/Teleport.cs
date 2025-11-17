using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private HashSet<GameObject> teleportObject = new HashSet<GameObject>();

    [SerializeField] private Transform destination;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (teleportObject.Contains(collision.gameObject))
        {
            return;
        }

        if (destination.TryGetComponent(out Teleport destinationTeleport)) 
        {
            destinationTeleport.teleportObject.Add(collision.gameObject);
        }

        collision.transform.position = destination.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        teleportObject.Remove(collision.gameObject);
    }
}
