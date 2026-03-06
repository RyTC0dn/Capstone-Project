using UnityEngine;

public enum Character
{
    Knight, 
    Cleric,
    Huntress,
    Wizard
}

public class CharacterSelect : MonoBehaviour
{
    public static Character selectCharacter {  get; private set; }

    public Character characterType;

    [Header("Selection Check")]
    public bool isKnight, isCleric, isHuntress, isWizard = false;

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
                isKnight=false;
                isCleric=false;
                isHuntress=false;
                isWizard=false;
                break;
        }
    }
}
