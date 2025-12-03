using UnityEngine;

public class BasicEnemyAttackState : MonoBehaviour
{
    private GameObject playerPos;
    private Rigidbody2D rb2D;
    [SerializeField]private bool isGrounded;
    private float raycastLength = 2;
    public int damage = 1;
    public float jumpForce;

    private Animator animator;
    private Vector2 lastPosition;

    private enum enemyTypes { ground, flying, spider}
    enemyTypes currentEnemyType;

    public GameEvent onAttackEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        playerPos = GameObject.FindGameObjectWithTag("Player");

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (currentEnemyType)
        {
            case enemyTypes.ground:
                GroundEnemy();
                break;
            case enemyTypes.flying:
                FlyingEnemy();
                break;
            case enemyTypes.spider:
                SpiderEnemy();
                break;
        }

        if (gameObject.tag == "GroundEnemy") { UpdateAnimation(); currentEnemyType = enemyTypes.ground; }
        if (gameObject.tag == "FlyingEnemy") { currentEnemyType = enemyTypes.flying; }
        if (gameObject.tag == "SpiderEnemy") { currentEnemyType = enemyTypes.spider; }

        
    }

    private void UpdateAnimation()
    {
        Vector2 currentPosition = transform.position;
        Vector2 velocity = (currentPosition - lastPosition) / Time.deltaTime;

        float horizontal = velocity.x;

        animator.SetFloat("horizontal", horizontal);

        lastPosition = currentPosition;

        if(horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if(horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void GroundEnemy()
    {        
        //Move towards player
        float groundEnemySpeed = GetComponent<BasicEnemyControls>().enemySpeed;
        
        Vector2 playerPosX = new Vector2(playerPos.transform.position.x, rb2D.position.y);
        rb2D.position = Vector2.MoveTowards(rb2D.position, playerPosX, groundEnemySpeed * Time.deltaTime);
    }

    public void SpiderEnemy()
    {
        //Move towards player
        float groundEnemySpeed = GetComponent<BasicEnemyControls>().enemySpeed;

        Vector2 playerPosX = new Vector2(playerPos.transform.position.x, rb2D.position.y);
        rb2D.position = Vector2.MoveTowards(rb2D.position, playerPosX, groundEnemySpeed * Time.deltaTime);

        float jump = jumpForce;

        if (Vector2.Distance(transform.position, playerPosX) <= 2)
        {
            //Jump
            rb2D.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        }        
    }

    public void FlyingEnemy()
    {
        float flyingEnemySpeed = GetComponent<BasicEnemyControls>().enemySpeed;
        transform.position = Vector2.MoveTowards(rb2D.position, playerPos.transform.position, flyingEnemySpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Raise a global event to attack player
            onAttackEvent.Raise(this, damage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Raise a global event to attack player
            onAttackEvent.Raise(this, damage);
        }
    }
}
