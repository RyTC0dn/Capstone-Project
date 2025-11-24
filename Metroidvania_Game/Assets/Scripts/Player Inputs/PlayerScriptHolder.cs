using UnityEngine;

public class PlayerScriptHolder : MonoBehaviour
{
    PrototypePlayerMovementControls movement;
    PrototypePlayerAttack attack;
    Knight_Ability2_AxeThrow ranged;
    PlayerSpawnControl spawnControl;
    PlayerJumps jump;

    public bool isActive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<PrototypePlayerMovementControls>();
        attack = GetComponent<PrototypePlayerAttack>();
        ranged = GetComponent<Knight_Ability2_AxeThrow>();
        spawnControl = GetComponent<PlayerSpawnControl>();
        jump = GetComponent<PlayerJumps>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool EnableScript()
    {
        movement.enabled = true;
        attack.enabled = true;
        ranged.enabled = true;
        spawnControl.enabled = true;
        jump.enabled = true;

        return isActive;
    }

    public bool DisableScript()
    {
        movement.enabled = false;
        attack.enabled = false;
        ranged.enabled = false;
        spawnControl.enabled = false;
        jump.enabled = false;

        return !isActive;
    }
}
