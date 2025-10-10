using UnityEngine;
using UnityEngine.InputSystem;

public class ElevatorFunction : MonoBehaviour
{
    //public Transform enterDoor;
    public Transform exitDoor;
    [SerializeField] private bool isNearElevator = false;

    private bool savedNPC;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        savedNPC = GameManager.instance.hasSavedBlacksmith;
        if (collision.CompareTag("Player") && !isNearElevator)
        {
            isNearElevator = true;

            if(savedNPC)
            {
                //Move the player to the exit door position
                Transform playerPos = PrototypePlayerMovementControls.Instance.transform;
                playerPos.position = exitDoor.transform.position;
            }          
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
