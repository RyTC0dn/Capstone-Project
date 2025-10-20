using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraZones : MonoBehaviour
{
    public Rooms roomType;
    public float minX, minY, maxX, maxY;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraControl cam = Camera.main.GetComponent<CameraControl>();
            if (cam != null)
            {
                cam.SetBounds(minX, maxX, minY, maxY, roomType);
            }
        }
    }
}
