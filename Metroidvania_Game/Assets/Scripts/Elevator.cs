using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
    public GameEvent UIActive;

    [Header("Elevator Animations")]
    [HideInInspector]public Animator elevatorAnimation;
    private bool elevatorOpened = false;
    private bool playerTeleported = false;


    [Header("UI Setup")]
    public GameObject parentPanel; [Tooltip("Manually assign from ElevatorManager prefab")]
    PrototypePlayerMovementControls playerControls;
    [Tooltip("Manually assign each elevator button from the panel on ElevatorManager")]
    public Button[] elevatorButtons;

    public TextMeshProUGUI inputText;

    private PrototypePlayerAttack playerAttack;

    public float buttonSpacing = -50f;
    private bool isNear = false;

    private void Start()
    {
        elevatorAnimation = GetComponent<Animator>();
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();
        playerAttack = FindAnyObjectByType<PrototypePlayerAttack>();


        //Disable all buttons at start
        foreach (Button button in elevatorButtons)
        {
            string destinationName = button.name;
            button.onClick.AddListener(() => OnButtonClicked(destinationName));
            button.interactable = false;
        }

        parentPanel.SetActive(false);
        inputText.enabled = false;

        //If this elevator name exists in the save data, register it automatically
        if (ElevatorManager.instance.saveData.
            registeredElevators.Contains(elevatorLocationName))
        {
            ElevatorManager.instance.RegisterElevator(this);
        }
    }

    private void Update()
    {
        //Check every frame if elevators have been registered
        foreach (Button button in elevatorButtons)
        {
            string destinationName = button.name;

            if (ElevatorManager.instance.elevators.ContainsKey(destinationName))
            {
                button.interactable = ElevatorManager.instance.elevators.ContainsKey(destinationName);

                PlayerPrefs.GetString("ElevatorRegistered", elevatorLocationName);                              
            }
        }
        TextColor();

        if (ElevatorManager.instance.elevators.ContainsKey(elevatorLocationName))
            inputText.enabled = true;
    }

    void TextColor()
    {
        Color color = inputText.color;
        float gradient = Mathf.PingPong(Time.time * 4f, 1f) + 1f;
        inputText.color = new Color(gradient, color.g, color.b);
    }

    public void OnInteract(Component sender, object data)
    {
        if (data is bool interact && interact && isNear)
        {
            if (ElevatorManager.instance.elevators.Count > 1)
            {   
                playerAttack.DisableAttack();

                elevatorAnimation.SetBool("isOpen", true);
                elevatorOpened = true;
                parentPanel.SetActive(true);

                UIActive.Raise(this, true);
            }
        }
    }

    public void OnButtonClicked(string destinationName)
    {
        StartCoroutine(CloseDoor(destinationName));                
    }

    private IEnumerator CloseDoor(string destinationName)
    {
        elevatorAnimation.SetBool("isOpen", false);
        yield return new WaitForSeconds(0.5f);

        //Wait for 0.5 seconds before teleporting player so animation can play
        ElevatorManager.instance.TeleportPlayer(destinationName, playerControls.transform);
        parentPanel.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNear = true;
            ElevatorManager.instance.RegisterElevator(this);
            inputText.enabled = true;
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
