using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class ElevatorManager : MonoBehaviour
{
    public static ElevatorManager instance {  get; private set; }

    public Dictionary<string, Elevator> elevators = new Dictionary<string, Elevator>();
    private Elevator currentElevator;
    public bool isNearElevator = false;

    [SerializeField]private string currentElevatorName;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterElevator(Elevator elevator)
    {
        if(!elevators.ContainsKey(elevator.elevatorLocationName))
        {
            elevators.Add(elevator.elevatorLocationName, elevator);            
        }
    }

    public void SetElevator(Elevator elevator)
    {
        currentElevator = elevator;
    }

    public void TeleportPlayer(string destinationName, Transform player)
    {
        if (elevators.ContainsKey(destinationName))
        {
            Vector3 targetPos = elevators[destinationName].transform.position;
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if(rb != null)
                rb.position = targetPos;
            else
                player.position = targetPos;
            Debug.Log("Has Teleported");
        }
        else
        {
            Debug.LogWarning($"Destination {destinationName} not found");
        }
    }

    public void DisablePlayerControl()
    {

    }

    public void EnablePlayerControl()
    {

    }
}
