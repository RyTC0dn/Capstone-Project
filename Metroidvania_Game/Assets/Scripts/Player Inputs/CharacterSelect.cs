using Unity.Behavior;
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

    public void SelectingCharacter(int selector)
    {
        if (selector == 0)
        {
            CharacterSelection(Character.Knight);
        }
        else if (selector == 1)
        {
            CharacterSelection(Character.Cleric);
        }
        else if (selector == 2)
        {
            CharacterSelection(Character.Huntress);
        }
        else if (selector == 3)
        {
            CharacterSelection(Character.Wizard);
        }
    }

    public void CharacterSelection(Character selection)
    {
        selectCharacter = selection;
    }

    private void ToggleSelectorUI(bool toggle)
    {
        selectorUI.SetActive(toggle);
    }
}