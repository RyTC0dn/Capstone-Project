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
        SpawnPoint point = FindAnyObjectByType<SpawnPoint>();
        string spawnName = GameManager.instance.nextSpawnPointName;
        savedNPC = GameManager.instance.hasSavedBlacksmith;
        if (savedNPC && Keyboard.current.eKey.isPressed && isNearElevator)
        {
            if(point.spawnName ==  spawnName)
            {
                //Move the player to the exit door position
                Transform playerPos = playerControls.transform;
                playerPos.position = point.transform.position;
            }            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.CompareTag("Player"))
        {
            isNearElevator = true;
            GameManager.instance.nextSpawnPointName = "_Elevator_";
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
