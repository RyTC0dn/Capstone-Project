using UnityEngine;

public class FallingBlocks : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject rock;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float maxRocks;
    [SerializeField] private float detectionRange;
    [SerializeField] private bool detectPlayer = false;
    [Space(20)]

    [SerializeField] private float damage;
    [SerializeField] private GameEvent damagePlayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        //For now, have the rocks fall when the player is detected below with the raycast
        detectPlayer = Physics2D.Raycast(transform.position, Vector2.down, detectionRange, LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position, Vector2.down * detectionRange, Color.red);

        if (detectPlayer)
        {
            SpawnRocks();
            detectPlayer = false; //Reset detection to prevent continuous spawning
        }
    }

    private void SpawnRocks()
    {
        if (rock == null) return;

        //Before the max amount of rocks spawn, spawn rocks at random x offsets from the spawn point
        for (int i = 0; i < maxRocks; i++)
        {
            //If the spawn point is not null, use its position, otherwise use the current position
            Vector2 offset = (spawnPoint != null) ? spawnPoint.position : transform.position;
            offset.x += Random.Range(-1f, 1f);
            Instantiate(rock, spawnPoint.position, Quaternion.identity);
        }
    }
}
