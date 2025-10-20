using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class ElevatorManager : MonoBehaviour
{
    public static ElevatorManager instance {  get; private set; }

    public Dictionary<string, Elevator> elevators = new Dictionary<string, Elevator>();
    private Elevator currentElevator;
    public bool isNearElevator = false;
    public bool isActive = false;

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
        player.position = elevators[destinationName].transform.position;
    }
}
