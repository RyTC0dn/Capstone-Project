using UnityEngine;
using UnityEngine.InputSystem;

public class ElevatorFunction : MonoBehaviour
{
    //public Transform enterDoor;
    public Transform exitDoor;
    [SerializeField] private bool isNearElevator = false;
    PrototypePlayerMovementControls playerMovementControls;

    private Transform player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovementControls = FindAnyObjectByType<PrototypePlayerMovementControls>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isNearElevator)
        {
            //Move the elevator towards set points
            float speed = 5 * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, exitDoor.position, speed);

            if(Vector2.Distance(transform.position, exitDoor.position) < 0.05f)
            {
                isNearElevator=false;

                
                if(player != null)
                {
                    player.SetParent(null);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //Parent the player to the elevator so they move together
            player = collision.collider.transform;
            player.SetParent(transform);
            isNearElevator = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //Unparent player when stepping off elevator
           collision.collider.transform.SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNearElevator = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Vector2.Distance(transform.position, exitDoor.position) <= 0.1f)
        {
            isNearElevator = false;
        }
    }
}
