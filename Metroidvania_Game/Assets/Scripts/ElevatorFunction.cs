using UnityEngine;
using UnityEngine.InputSystem;

public class ElevatorFunction : MonoBehaviour
{
    //public Transform enterDoor;
    public Transform exitDoor;
    [SerializeField] private bool isNearElevator = false;
    PrototypePlayerMovementControls playerMovementControls;


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
            float speed = 5 * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, exitDoor.position, speed);
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
