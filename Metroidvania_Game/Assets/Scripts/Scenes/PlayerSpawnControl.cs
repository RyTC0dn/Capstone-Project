using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnControl : MonoBehaviour
{
    /// <summary>
    /// How the sequence setup works for the spawn point: 
    /// 1. Place spawn point script in spawn point object, noting down objects name
    /// 2. Make sure to store the next spawn point name between scenes or within level 
    /// and have it call back to GameManager.instance.spawnName
    /// Will be further developed
    /// </summary>

    [SerializeField]private bool isNearElevator = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //This section is dedicated to the spawning points of the player within the town hub area
        string spawnName = GameManager.instance.nextSpawnPointName;

        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        foreach(SpawnPoint point in spawnPoints)
        {
            if(point.spawnName == spawnName)
            {
                transform.position = point.transform.position;
                return;
            }
        }
        Debug.LogWarning($"SpawnPoint {spawnName} has not been found in scene");
    }

    //This section is to test the elevator teleportation script
    private void Update()
    {
        if (isNearElevator && Keyboard.current.eKey.isPressed)
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Elevator"))
        {
            isNearElevator=true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Elevator"))
        {
            isNearElevator = false;
        }
    }
}
