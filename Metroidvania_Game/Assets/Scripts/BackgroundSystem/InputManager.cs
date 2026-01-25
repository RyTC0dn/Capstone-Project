using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public bool MenuOpenCloseInput {  get; private set; }

    public bool InventoryOpenCloseInput { get; private set; }

    private PlayerInput playerInput;

    private InputAction menuOpenCloseAction;

    private InputAction inventoryOpenCloseAction;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
            
        //Initialize input actions from player input map by calling the action name
        playerInput = GetComponent<PlayerInput>();
        menuOpenCloseAction = playerInput.actions["MenuOpenClose"];
        inventoryOpenCloseAction = playerInput.actions["InventoryMenuOpenClose"];
    }

    private void Update()
    {
        //Booleans that check the button press 
        MenuOpenCloseInput = menuOpenCloseAction.WasPressedThisFrame();
        InventoryOpenCloseInput = inventoryOpenCloseAction.WasPressedThisFrame();
    }
}
