using UnityEngine;
using UnityEngine.InputSystem;

public enum Character
{
    Knight, 
    Priest, 
    Huntress, 
    Mage
}

public class CharacterManager : MonoBehaviour
{
    [Header("Characters")]
    private Animator characterAnim;
    public Character character = Character.Knight;
    public GameObject characterCanvas;

    [Header("Knight Scripts")]
    Knight_Ability1_Shield shield;
    PrototypePlayerAttack attack;
    PrototypePlayerMovementControls controls;
    Knight_Ability2_AxeThrow attackRanged;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       characterAnim = GetComponent<Animator>();
        characterCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.tabKey.isPressed)
        {
            characterCanvas.SetActive(true);
        }
    }

    public void KnightFunctions()
    {
        PrototypePlayerAttack attack = gameObject.GetComponent<PrototypePlayerAttack>();
        characterAnim.SetInteger("Character", 0);
        characterCanvas.SetActive(false);
    }

    public void PriestFunctions()
    {
        characterAnim.SetInteger("Character", 1);
        characterCanvas.SetActive(false);
    }
}
