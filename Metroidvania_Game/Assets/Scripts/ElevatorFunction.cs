using UnityEngine;
using UnityEngine.InputSystem;

public class ElevatorFunction : MonoBehaviour
{
    //public Transform enterDoor;
    public Transform exitDoor;
    [SerializeField] private bool isNearElevator = false;

    PrototypePlayerMovementControls playerControls;

    private bool savedNPC;


    private void Start()
    {
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();
    }

    private void Update()
    {
        savedNPC = GameManager.instance.hasSavedBlacksmith;
        if (savedNPC && Keyboard.current.eKey.isPressed && isNearElevator)
        {
            //Move the player to the exit door position
            Transform playerPos = playerControls.transform;
            playerPos.position = exitDoor.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.CompareTag("Player") && !isNearElevator)
        {
            isNearElevator = true;                 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNearElevator = false;
        }
    }
}
