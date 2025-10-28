using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;

public class ElevatorManager : MonoBehaviour
{
    public static ElevatorManager instance {  get; private set; }

    private readonly List<Elevator> elevatorList = new List<Elevator>();
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
            elevatorList.Add(elevator);
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

    //Get the elevator above or below based on the order in which they were registered
    public Elevator GetNextElevator(Elevator current, bool goingUp)
    {
        int currentIndex = elevatorList.IndexOf(current);
        if (currentIndex < 0) return null;

        if(goingUp && currentIndex < elevatorList.Count - 1)
            return elevatorList[currentIndex + 1];
        if (!goingUp && currentIndex > 0)
            return elevatorList[currentIndex - 1];

        return null; //No elevator in that direction
    }

    public int ElevatorCount => elevatorList.Count;

}
