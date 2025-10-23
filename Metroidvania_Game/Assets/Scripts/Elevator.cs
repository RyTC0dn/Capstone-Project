using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Elevator : MonoBehaviour
{
    /// <summary>
    /// The main way that the elevator works in tandem with the elevator manager and player spawn control scripts.
    /// To summarize, you define which location you want the player to go to, elevator location name equals the name of the game object.
    /// Give the generate button function the name of the elevator you want players to move to. 
    /// Then the script will compare the name of the button to the name of the gameobject/elevatorlocationname, if they match then
    /// move the player to that location.
    /// </summary>
    public string elevatorLocationName;

    [Header("Elevator Animations")]
    private Animator elevatorAnimation;

    [Header("UI Setup")]
    public GameObject buttonPrefab;
    public GameObject parentPanel;
    PrototypePlayerMovementControls playerControls;

    public float buttonSpacing = -50f;

    private void Start()
    {         
        elevatorAnimation = GetComponent<Animator>();
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();

        GenerateButton("Elevator_Entrance", 1);
        GenerateButton("Elevator_A2", 4);
        parentPanel.SetActive(false);
    }

    private void Update()
    {
        ElevatorManager.instance.RegisterElevator(this);//Assigning this elevator object in elevator list
        if (ElevatorManager.instance.isActive)
        {
            elevatorAnimation.SetTrigger("OpenDoor");
            parentPanel.SetActive(true);
        }
        else if(!ElevatorManager.instance.isActive)
        {
            parentPanel.SetActive(false);
        }
        
    }

    void GenerateButton(string destinationName, int index)
    {
        GameObject newButton = Instantiate(buttonPrefab, parentPanel.transform);

        RectTransform rect = newButton.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, index * buttonSpacing);

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
        ElevatorManager.instance.TeleportPlayer(destinationName, playerControls.transform);
        ElevatorManager.instance.isActive = false;
        elevatorAnimation.SetTrigger("CloseDoor");
        if (ElevatorManager.instance.elevators.ContainsKey(destinationName))
        {
           
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
