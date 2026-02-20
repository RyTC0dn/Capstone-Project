using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ElevatorSaveData
{
    public List<string> registeredElevators = new List<string>();
}

public class ElevatorManager : MonoBehaviour
{
    public static ElevatorManager instance { get; private set; }

    public Dictionary<string, Elevator> elevators = new Dictionary<string, Elevator>();
    private Elevator currentElevator;
    public bool isNearElevator = false;
    public bool isActive = false;

    public GameObject parentPanel;

    public ElevatorSaveData saveData = new ElevatorSaveData();

    public GameObject elevatorFirst;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("ElevatorRegistered"))
        {
            string json = PlayerPrefs.GetString("ElevatorRegistered");
            saveData = JsonUtility.FromJson<ElevatorSaveData>(json);
        }
    }

    public void CloseUI()//Close UI on button click
    {
        //Disable event sytem controls
        EventSystem.current.SetSelectedGameObject(null);

        parentPanel.SetActive(false);
        Invoke(nameof(EnableComponent), 0.3f);
    }

    public void RegisterElevator(Elevator elevator)
    {
        if (!elevators.ContainsKey(elevator.elevatorLocationName))
        {
            elevators.Add(elevator.elevatorLocationName, elevator);

            saveData.registeredElevators.Add(elevator.elevatorLocationName);

            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString("ElevatorRegistered", json);
            PlayerPrefs.Save();

            Debug.Log($"Registered elevator: {elevator.elevatorLocationName}");
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
            //Disable event sytem controls
            EventSystem.current.SetSelectedGameObject(null);

            Vector3 targetPos = elevators[destinationName].transform.position;
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.position = targetPos;
            else
                player.position = targetPos;
            Debug.Log("Has Teleported");
            Invoke(nameof(EnableComponent), 0.3f);
        }
        else
        {
            Debug.LogWarning($"Destination {destinationName} not found");
        }
    }

    void EnableComponent()
    {
        var playerAttack = FindAnyObjectByType<Player_Attack_Knight>();
        playerAttack.EnableAttack();
    }
}
