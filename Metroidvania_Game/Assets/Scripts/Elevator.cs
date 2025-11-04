using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameEvent teleportPlayer;

    [Header("Elevator Animations")]
    private Animator elevatorAnimation;


    [Header("UI Setup")]
    public GameObject parentPanel; [Tooltip("Manually assign from ElevatorManager prefab")]
    PrototypePlayerMovementControls playerControls;
    [Tooltip("Manually assign each elevator button from the panel on ElevatorManager")]
    public Button[] elevatorButtons; 

    public float buttonSpacing = -50f;
    private bool isNear = false;

    private void Start()
    {
        elevatorAnimation = GetComponent<Animator>();
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();

        //Disable all buttons at start
        foreach (Button button in elevatorButtons)
        {
            button.interactable = false;
        }

        parentPanel.SetActive(false);
    }

    private void Update()
    {
        //Check every frame if elevators have been registered
        foreach (Button button in elevatorButtons)
        {
            string destinationName = button.name;

            if (ElevatorManager.instance.elevators.ContainsKey(destinationName))
            {
                button.interactable = true;

                //Ensure that listener is only added once
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnButtonClicked(destinationName));
            }
        }

    }
    public void OnInteract(Component sender, object data)
    {
        if (data is bool interact && interact && isNear)
        {
            if (ElevatorManager.instance.elevators.Count > 1)
            {
                elevatorAnimation.SetTrigger("OpenDoor");
                parentPanel.SetActive(true);
                Debug.Log("Event recieved");
            }
        }
    }

    public void OnButtonClicked(string destinationName)
    {
        ElevatorManager.instance.TeleportPlayer(destinationName, playerControls.transform);
        parentPanel.SetActive(false);
        elevatorAnimation.SetTrigger("CloseDoor");
    }
  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNear = true;
            ElevatorManager.instance.RegisterElevator(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNear = false;
        }
    }
}
