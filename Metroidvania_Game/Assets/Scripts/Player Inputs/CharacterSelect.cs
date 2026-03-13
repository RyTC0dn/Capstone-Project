using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum Character
{
    Knight,
    Cleric,
    Huntress,
    Wizard
}

public class CharacterSelect : MonoBehaviour
{
    public static Character selectCharacter { get; private set; }

    private Animator animator;
    public GameObject selectorUI;
    public GameObject iconFirst; //First button selected for the controller navigation
    private bool isSelectorActive = false;

    //Attack script references
    public Player_Knight_Attack knight_Attack;

    //Call the cleric attack script here

    [Header("Selection Check")]
    public bool isKnight, isCleric, isHuntress, isWizard = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        selectorUI.SetActive(false);
    }

    private void Update()
    {
        #region Singleton Pattern

        // Check for input to toggle the character selector UI
        bool activeInput = Keyboard.current.iKey.wasPressedThisFrame || Gamepad.current?.dpad.up.wasPressedThisFrame == true;

        if (activeInput)
        {
            if (isSelectorActive)
            {
                Time.timeScale = 1f; // Resume the game when the selector is closed
                ToggleSelectorUI(false);
                isSelectorActive = false;

                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                Time.timeScale = 0f; // Pause the game when the selector is active
                ToggleSelectorUI(true);
                isSelectorActive = true;

                //Set the first button to be selected for controller navigation
                EventSystem.current.SetSelectedGameObject(iconFirst);
            }
        }

        #endregion Singleton Pattern
    }

    public void SelectingCharacter(Character chosenCharacter)
    {
        switch (chosenCharacter)
        {
            case Character.Knight:
                isKnight = true;
                break;

            case Character.Cleric:
                isCleric = true;
                break;

            case Character.Huntress:
                isHuntress = true;
                break;

            case Character.Wizard:
                isWizard = true;
                break;

            default:
                isKnight = false;
                isCleric = false;
                isHuntress = false;
                isWizard = false;
                break;
        }
    }

    private void ToggleSelectorUI(bool toggle)
    {
        selectorUI.SetActive(toggle);
    }
}