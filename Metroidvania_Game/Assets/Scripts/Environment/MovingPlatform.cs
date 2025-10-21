using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform posA;
    [SerializeField] Transform posB;

    [SerializeField] float moveSpeed = 2f;


    private Vector3 nextPosition;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextPosition = posB.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

        if (transform.position == nextPosition)
        {
            nextPosition = (nextPosition == posA.position) ? posB.position : posA.position;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
