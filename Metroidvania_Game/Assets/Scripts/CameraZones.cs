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

    private void OnDrawGizmos()
    {
        //Drawing rectangle boundary that the camera moves in

        Gizmos.color = new Color(255, 0, 0);

        //Top line
        Vector2 topLeft = new Vector2(minX, maxY);
        Vector2 topRight = new Vector2(maxX, maxY);
        Gizmos.DrawLine(topLeft, topRight);

        //Bottom line
        Vector2 bottomLeft = new Vector2(minX, minY);
        Vector2 bottomRight = new Vector2(maxX, minY);
        Gizmos.DrawLine(bottomLeft, bottomRight);

        //Left line
        Vector2 leftTop = new Vector2(minX, maxY);
        Vector2 leftBottom = new Vector2(minX, minY);
        Gizmos.DrawLine(leftTop, leftBottom);

        //Right line
        Vector2 rightTop = new Vector2(maxX, maxY);
        Vector2 rightBottom = new Vector2(maxX, minY);
        Gizmos.DrawLine(rightTop, rightBottom);
    }
}
