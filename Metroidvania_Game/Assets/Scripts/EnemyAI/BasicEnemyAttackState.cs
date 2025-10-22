using UnityEngine;

public class BasicEnemyAttackState : MonoBehaviour
{
    private GameObject playerPos;
    PrototypePlayerMovementControls playerControls;
    private Rigidbody2D rb2D;
    [SerializeField]private bool isGrounded;
    private float raycastLength = 2;
    public int damage = 1;

    private enum enemyTypes { ground, flying}
    enemyTypes currentEnemyType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        playerPos = GameObject.FindGameObjectWithTag("Player");

        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = GetComponent<BasicEnemyControls>().enemySpeed;
        switch (currentEnemyType)
        {
            case enemyTypes.ground:
                GroundEnemy();
                break;
            case enemyTypes.flying:
                FlyingEnemy();
                break;
        }

        if (gameObject.tag == "GroundEnemy") { currentEnemyType = enemyTypes.ground; }
        if (gameObject.tag == "FlyingEnemy") { currentEnemyType = enemyTypes.flying; }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.PlayerDamaged(damage);
            playerControls.gameObject.transform.position = GameManager.instance.playerSpawnPoint.transform.position;
        }
    }
}
