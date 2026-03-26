using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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
    [HideInInspector] public Animator elevatorAnimation;

    private bool elevatorOpened = false;
    private bool playerTeleported = false;

    [Header("UI Setup")]
    public GameObject parentPanel; [Tooltip("Manually assign from ElevatorManager prefab")]
    private PrototypePlayerMovementControls playerControls;

    [Tooltip("Manually assign each elevator button from the panel on ElevatorManager")]
    public Button[] elevatorButtons;

    public TextMeshProUGUI inputText;

    private Player_Knight_Attack playerAttack;

    public float buttonSpacing = -50f;
    public float messageTimer = 1;
    private bool isNear = false;

    private void Start()
    {
        elevatorAnimation = GetComponent<Animator>();
        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();
        playerAttack = FindAnyObjectByType<Player_Knight_Attack>();

        //Disable all buttons at start
        foreach (Button button in elevatorButtons)
        {
            string destinationName = button.name;
            button.onClick.AddListener(delegate { OnButtonClicked(destinationName); });
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
        //If the shop is active ensure this is not reading click input
        if (GameManager.instance.state == GameStates.Pause)
        {
            return;
        }

        TextColor();

        if (ElevatorManager.instance.elevators.ContainsKey(elevatorLocationName))
            inputText.enabled = true;
    }

    private void FixedUpdate()
    {
        InputListener();
    }

    private void TextColor()
    {
        Color color = inputText.color;
        float gradient = Mathf.PingPong(Time.time * 4f, 1f) + 1f;
        inputText.color = new Color(gradient, color.g, color.b);
    }

    private void InputListener()
    {
        bool input = Keyboard.current?.eKey.wasPressedThisFrame ?? false;
        bool game = Gamepad.current?.buttonWest.wasPressedThisFrame ?? false;
        bool pressed = input || game;

        if (isNear && pressed)
        {
            if (ElevatorManager.instance.elevators.Count > 1)
            {
                OpenElevatorUI();
            }
            else
            {
                ElevatorManager.instance.textPopup.SetActive(true);
                ElevatorManager.instance.inputCount++;
            }
        }
        else if (pressed && ElevatorManager.instance.inputCount >= 2)
        {
            ElevatorManager.instance.textPopup.SetActive(false);
            ElevatorManager.instance.inputCount = 0;
        }
    }

    //public void OnInteract(Component sender, object data)
    //{
    //    if (data is bool interact && interact && isNear)
    //    {
    //        if (ElevatorManager.instance.elevators.Count > 1)
    //        {
    //            OpenElevatorUI();
    //        }
    //    }
    //}

    private void OpenElevatorUI()
    {
        parentPanel.SetActive(true);

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

        //Set default UI button once
        EventSystem.current.SetSelectedGameObject(ElevatorManager.instance.elevatorFirst);

        playerAttack.DisableAttack();

        elevatorAnimation.SetBool("isOpen", true);
        elevatorOpened = true;
    }

    public void OnButtonClicked(string destinationName)
    {
        StartCoroutine(CloseDoor(destinationName));
    }

    private IEnumerator CloseDoor(string destinationName)
    {
        elevatorAnimation.SetBool("isOpen", false);
        ElevatorManager.instance.isActive = true;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNear = true;
            ElevatorManager.instance.SetElevator(this);
            ElevatorManager.instance.elevatorCam.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y,
                this.transform.position.z - 10);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNear = false;
            ElevatorManager.instance.SetElevator(null);
        }
    }
}