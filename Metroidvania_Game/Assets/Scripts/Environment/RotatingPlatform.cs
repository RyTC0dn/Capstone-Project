using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public GameObject objectToOrbit;
    public float angle;
    public float radius = 10;
    public float degreesPerSecond = 30;

    private void Update()
    {
        angle += degreesPerSecond * Time.deltaTime;

        if (angle > 360)
        {
            angle -= 360;
        }

        Vector3 orbit = Vector3.up * radius;
        orbit = Quaternion.Euler(0, 0, angle) * orbit;

        transform.position = objectToOrbit.transform.position + orbit;
    }

    //When the player is colliding with the platform, they will become a child
    // of the platform so that they will move with it

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }


    //When the player stops colliding with the platform, they will no longer be a child
    // of the platform
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
