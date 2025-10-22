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
    public PlayableDirector elevatorTimeline;
    private bool isSequencing = false;

    [Header("UI Setup")]
    public GameObject buttonPrefab;
    public GameObject parentPanel;
    public GameObject player;

    public float buttonSpacing = -50f;

    private void Start()
    {        
        ElevatorManager.instance.RegisterElevator(this);//Assigning this elevator object in elevator list
        elevatorAnimation = GetComponent<Animator>();

        GenerateButton("Elevator_Entrance", 1);
        GenerateButton("Elevator_A2", 4);
        parentPanel.SetActive(false);
    }

    private void Update()
    {
        if (ElevatorManager.instance.isActive && !isSequencing)
        {
            elevatorAnimation.SetTrigger("OpenDoor");
            parentPanel.SetActive(true);
        }
        else if(!ElevatorManager.instance.isActive && !isSequencing)
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
        
        if(ElevatorManager.instance.elevators.ContainsKey(destinationName))
        {
            ElevatorManager.instance.TeleportPlayer(destinationName, player.transform);
            ElevatorManager.instance.isActive = false;
            elevatorAnimation.SetTrigger("CloseDoor");
        }
    }

    private IEnumerator PlayElevatorSequence(string destinationName)
    {
        isSequencing = true;
        ElevatorManager.instance.DisablePlayerControl();

        //Play door close animation
        elevatorAnimation.SetTrigger("CloseDoor");
        yield return new WaitForSeconds(1);

        //Play the elevator timeline sequence
        elevatorTimeline.Play();
        yield return new WaitForSeconds((float)elevatorTimeline.duration);

        //Teleport the player
        ElevatorManager.instance.TeleportPlayer(destinationName, player.transform);

        //Elevator doors open after teleport
        elevatorAnimation.SetTrigger("OpenDoor");
        yield return new WaitForSeconds(1f);

        ElevatorManager.instance.EnablePlayerControl();
        isSequencing = false;
        ElevatorManager.instance.isActive = false;
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
