using UnityEngine;
using UnityEngine.InputSystem;

public class ControlScheme : MonoBehaviour
{
    public static ControlScheme instance { get; private set; }

    [HideInInspector] public float moveInputX;

    private PlayerInput _playerInput;
    private InputAction _movementAction;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        if (_playerInput == null)
        {
            Debug.LogWarning("ControlScheme requires a PlayerInput component on the same GameObject.", this);
            return;
        }

        _movementAction = _playerInput.actions.FindAction("Movement", true);
        _movementAction.started += Move;
        _movementAction.performed += Move;
        _movementAction.canceled += Move;
    }

    private void OnDestroy()
    {
        if (_movementAction == null)
            return;

        _movementAction.started -= Move;
        _movementAction.performed -= Move;
        _movementAction.canceled -= Move;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInputX = context.canceled ? 0f : context.ReadValue<Vector2>().x;
    }
}
