using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

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

    [Header("Elevator Setup")]
    private Animator elevatorAnimation;
    [SerializeField]private bool isNear = false;
    [SerializeField]private bool isCurrentElevator;
    public GameEvent teleportPlayer;

    [Header("UI Setup")]
    public GameObject buttonPrefab;
    public GameObject parentPanel;
    PrototypePlayerMovementControls playerControls;

    public float buttonSpacing = -50f;

    private void Start()
    {         
        elevatorAnimation = GetComponent<Animator>();
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();

        parentPanel.SetActive(false);
    }

    private void Update()
    {
        if (ElevatorManager.instance.elevators.Count >= 0)
        {

        }            
    }

    public void OnInteract(Component sender, object data)
    {
        if (data is bool interact && interact && isNear)
        {
           if(ElevatorManager.instance.ElevatorCount > 1)
            {
                elevatorAnimation.SetTrigger("OpenDoor");
                ElevatorManager.instance.SetElevator(this);
                GenerateButtons();
                parentPanel.SetActive(true);
                Debug.Log("Event recieved");
            }
        }
    }

    void GenerateButtons()
    {
        //Clear existing buttons
        foreach (Transform child in parentPanel.transform)
            Destroy(child.gameObject);

        Elevator upElevator = ElevatorManager.instance.GetNextElevator(this, true);
        Elevator downElevator = ElevatorManager.instance.GetNextElevator(this, false);

        int index = 0;
        if(upElevator != null)
        {
            CreateButton("UP", upElevator.elevatorLocationName, index++);
        }

        if (downElevator != null)
            CreateButton("DOWN", downElevator.elevatorLocationName, index++);
    }
    private void CreateButton(string labelText, string destinationName, int index)
    {
        GameObject newButton = Instantiate(buttonPrefab, parentPanel.transform);

        RectTransform rect = newButton.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, index * buttonSpacing);

        TextMeshProUGUI label = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null)
        {
            label.text = labelText;
        }


        Button buttonComponent = newButton.GetComponent<Button>();
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(() => OnButtonClicked(destinationName));
        }
    }

    void OnButtonClicked(string destinationName)
    {

        ElevatorManager.instance.TeleportPlayer(destinationName, playerControls.transform);
        elevatorAnimation.SetTrigger("CloseDoor");
        parentPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNear = true;
            ElevatorManager.instance.RegisterElevator(this);//Assigning this elevator object in elevator list   
            foreach (string key in ElevatorManager.instance.elevators.Keys)
            {
                Debug.Log($"Elevator" + key);
            }
           
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
