using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyObject; //Manually select which enemy you wish to spawn
    public GameObject spawnObject;
    private bool spawning = false;
    private SpriteRenderer sp;
    [Space(20)]

    [Tooltip("Assign rate of spawn for the enemies")]
    [SerializeField]private float spawnRate;

    public float timeUntilSpawn; //The amount of time it will take until spawn

    public int spawnCount;
    [SerializeField]private int maxSpawnCount; //Set the max amount of enemies that should spawn

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GameManager.instance.isBlackSmithSaved)
        {
            gameObject.SetActive(false);
        }

        SetSpawnTime();
        spawnObject.SetActive(false);
        sp = GetComponent<SpriteRenderer>();

        sp.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnCount < maxSpawnCount && spawning)
        {
            timeUntilSpawn -= Time.deltaTime;

            if (timeUntilSpawn <= 0)
            {
                Instantiate(enemyObject, spawnObject.transform.position, Quaternion.identity);
                spawnCount++;
                SetSpawnTime();
                return;
            }
        }
        else if (spawnCount >= maxSpawnCount && spawning)
        {
            spawning = false;
            sp.enabled = false;
        }
    }

    private void SetSpawnTime()
    {
        //Set the amount of spawn time the enemies will have here
        timeUntilSpawn = spawnRate;
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
