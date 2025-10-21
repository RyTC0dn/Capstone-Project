using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Elevator : MonoBehaviour
{
    public string elevatorLocationName;
    private Animator elevatorAnimation;

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
        if (ElevatorManager.instance.isActive)
        {
            elevatorAnimation.SetTrigger("inOperation");
            parentPanel.SetActive(true);
        }
        else
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
