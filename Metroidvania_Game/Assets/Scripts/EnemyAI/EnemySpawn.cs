using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyObject; //Manually select which enemy you wish to spawn
    public Collider2D roomCollider; //Get the collider of the room to trigger when player enters

    [SerializeField]private float minTimer; //Setting the min time for spawn

    [SerializeField]private float maxTimer; //Setting the max time for spawn 

    public float timeUntilSpawn; //The amount of time it will take until spawn

    public int spawnCount;
    [SerializeField]private int maxSpawnCount; //Set the max amount of enemies that should spawn

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetSpawnTime();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnCount <= maxSpawnCount)
        {
            timeUntilSpawn -= Time.deltaTime;

            if (timeUntilSpawn <= 0)
            {
                Instantiate(enemyObject, transform.position, Quaternion.identity);
                spawnCount++;
                SetSpawnTime();
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void SetSpawnTime()
    {
        //Randomize the time until spawn
        timeUntilSpawn = Random.Range(minTimer, maxTimer); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && )
        {

        }
    }
}
