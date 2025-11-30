using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyObject; //Manually select which enemy you wish to spawn
    public GameObject spawnObject;
    private bool spawning = false;
    private SpriteRenderer sp;

    [SerializeField]private float minTimer; //Setting the min time for spawn

    [SerializeField]private float maxTimer; //Setting the max time for spawn 

    public float timeUntilSpawn; //The amount of time it will take until spawn

    public int spawnCount;
    [SerializeField]private int maxSpawnCount; //Set the max amount of enemies that should spawn

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetSpawnTime();
        spawnObject.SetActive(false);
        sp = GetComponent<SpriteRenderer>();

        sp.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnCount <= maxSpawnCount && spawning)
        {
            timeUntilSpawn -= Time.deltaTime;

            if (timeUntilSpawn <= 0)
            {
                Instantiate(enemyObject, spawnObject.transform.position, Quaternion.identity);
                spawnCount++;
                SetSpawnTime();
            }
        }
        else
        {
            spawnObject.SetActive(false);
        }
    }

    private void SetSpawnTime()
    {
        //Randomize the time until spawn
        timeUntilSpawn = Random.Range(minTimer, maxTimer); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spawnObject.SetActive(true);
            sp.enabled = true;
            spawning = true;
        }
    }
}
