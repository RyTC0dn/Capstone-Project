using UnityEngine;

public class BasicEnemyAttackState : MonoBehaviour
{
    private GameObject playerPos;
    PrototypePlayerMovementControls playerControls;
    [SerializeField] private LayerMask playerLayer;
    private Rigidbody2D rb2D;
    [SerializeField]private bool isGrounded;
    public int damage = 1;
    private float attackRange = 10f;

    public GameObject idlePoint;
    private Vector2 startingPos;

    RaycastHit2D hit;

    private enum EnemyTypes { ground, flying}
    EnemyTypes currentEnemyType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        playerPos = GameObject.FindGameObjectWithTag("Player");

        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();

        if(CompareTag("GroundEnemy")) currentEnemyType = EnemyTypes.ground;
        else if(CompareTag("FlyingEnemy")) currentEnemyType= EnemyTypes.flying;

        startingPos = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerPos.transform.position);
        switch (currentEnemyType)
        {
            case EnemyTypes.ground:
                if (distanceToPlayer <= attackRange)
                    GroundEnemy();
                else
                    Idle();
                break;
            case EnemyTypes.flying:
                if (distanceToPlayer <= attackRange)
                    FlyingEnemy();                    
                else
                    Idle();
                break;
        }
    }

    public void GroundEnemy()
    {
        float groundEnemySpeed = GetComponent<BasicEnemyControls>().enemySpeed;
        
        Vector2 playerPosX = new Vector2(playerPos.transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(rb2D.position, playerPosX, groundEnemySpeed * Time.deltaTime);

        float jumpForce = 5;

        //Jump
        rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void FlyingEnemy()
    {
        float flyingEnemySpeed = GetComponent<BasicEnemyControls>().enemySpeed;
        transform.position = Vector2.MoveTowards(rb2D.position, playerPos.transform.position, flyingEnemySpeed * Time.deltaTime);
    }

    public void Idle()
    {
        float enemySpeed = GetComponent<BasicEnemyControls>().enemySpeed;
        transform.position = Vector2.MoveTowards(transform.position, startingPos, enemySpeed * Time.deltaTime);        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.PlayerDamaged(damage);
            playerControls.gameObject.transform.position = GameManager.instance.playerSpawnPoint.transform.position;
        }
    }
}
