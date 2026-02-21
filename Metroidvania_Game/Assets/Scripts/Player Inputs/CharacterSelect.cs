using UnityEngine;
using System.Collections;

public enum  CharacterType
{
    Knight, 
    Cleric
}



public class CharacterSelect : MonoBehaviour
{
    public static CharacterSelect Instance { get; private set; }
    public bool jumping { get; internal set; }

    [Header("Character Selector Setup")]
    public CharacterType selectedCharacter;
    private Animator animator;
    public GameObject selectorUI;

    //Script references
    Player_Attack_Cleric clericAtk;
    Player_Attack_Knight knightAtk;



    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        clericAtk = GetComponentInParent<Player_Attack_Cleric>();
        knightAtk = GetComponentInParent<Player_Attack_Knight>();
        selectorUI.SetActive(false);
    }

    private void CharacterUpdate()
    {
        switch (selectedCharacter)
        {
            case CharacterType.Knight:
                animator.SetBool("isKnight", true);
                knightAtk.enabled = true;
                clericAtk.enabled = false;
                break;
            case CharacterType.Cleric:
                animator.SetBool("isKnight", false);
                knightAtk.enabled = false;
                clericAtk.enabled = true;
                break;
            default:
                break;
        }
    }

    #region Script Handlers
    public void KnightSelect()
    {
        selectedCharacter = CharacterType.Knight;
        CharacterUpdate();
    }

    public void ClericSelect()
    {
        selectedCharacter = CharacterType.Cleric;
        CharacterUpdate();
    }

    //public void EnableScripts(Component script)
    //{
    //    script.enabled = true;
    //}

    //public void DisableScripts(Component script)
    //{
    //    script.enabled = false;
    //}
    #endregion

    public void ToggleSelectorUI(bool isActive)
    {
        selectorUI.SetActive(isActive);
    }
}
