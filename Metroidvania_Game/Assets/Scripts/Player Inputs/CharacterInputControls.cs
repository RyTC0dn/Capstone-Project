using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInputControls : MonoBehaviour
{
    /// <summary>
    /// This script is for setting the movement controls for keyboard and controller
    /// that can be cutomized 
    /// </summary>
    private Rigidbody2D rb2D;
    private PlayerInput inputs;
    private Vector2 moveInput;
    public float playerSpeed;

    private void Start()
    {
        inputs = GetComponent<PlayerInput>();
    }


    public void SetMovementControls(InputAction.CallbackContext movement)
    {
        moveInput = movement.ReadValue<Vector2>();
    }
}
