using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Elevator : MonoBehaviour
{
    public string elevatorLocationName;

    [Header("UI Setup")]
    public GameObject buttonPrefab;
    public Transform parentPanel;
    public GameObject player;

    private void Start()
    {        
        ElevatorManager.instance.RegisterElevator(this);//Assigning this elevator object in elevator list

        

        GenerateButton("Elevator_Entrance", parentPanel);
        GenerateButton("Elevator_A2", parentPanel);
    }

    private void Update()
    {
        
    }

    void GenerateButton(string destinationName, Transform position)
    {
        GameObject newButton = Instantiate(buttonPrefab, position);

        newButton.name = "Floor Button";

        TextMeshProUGUI label = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null )
        {
            label.text = destinationName;
        }
       

        Button buttonComponent = newButton.GetComponent<Button>();
        if( buttonComponent != null )
        {
            buttonComponent.onClick.AddListener(() => OnButtonClicked(destinationName));
        }
    }

    void OnButtonClicked(string destinationName)
    {
        if(ElevatorManager.instance.elevators.ContainsKey(destinationName))
        {
            ElevatorManager.instance.TeleportPlayer(destinationName, player.transform);
            Debug.Log("Teleported");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ElevatorManager.instance.isNearElevator = true;
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ElevatorManager.instance.isNearElevator = false;
        }        
    }
}
