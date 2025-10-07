using UnityEngine;

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
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            playerMovementControls.transform.position = exitDoor.position;
        }
    }
}
