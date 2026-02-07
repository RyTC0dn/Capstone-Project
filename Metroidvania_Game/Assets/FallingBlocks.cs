using UnityEngine;

public class FallingBlocks : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject rock;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float maxRocks;
    [SerializeField] private float detectionRange;
    [SerializeField] private bool detectPlayer;
    private float reInitializeTime = 2;
    private bool wasPlayerDetected;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
        if (wasPlayerDetected)
        {
            reInitializeTime -= Time.deltaTime;
            if (reInitializeTime <= 0f)
            {
                wasPlayerDetected = false;
                reInitializeTime = 2f; // Reset the timer
            }
        }
    }

    private void DetectPlayer()
    {
        if (wasPlayerDetected) return;

        //For now, have the rocks fall when the player is detected below with the raycast
        detectPlayer = Physics2D.Raycast(transform.position, Vector2.down, detectionRange, LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position, Vector2.down * detectionRange, Color.red);

        if (detectPlayer)
        {
            SpawnRocks();
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
            offset.x += Random.Range(-5f, 5f);

            Quaternion angle = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

            Instantiate(rock, offset, angle);
            wasPlayerDetected = true;
        }
    }

    private void OnDrawGizmos()
    {
        //Draw a ray downwards to show the detection range
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * detectionRange);
    }
}
