using UnityEngine;

public class BasicEnemyAttackState : MonoBehaviour
{
    private GameObject playerPos;
    private PrototypePlayerMovementControls playerControls;
    private Rigidbody2D rb2D;
    [SerializeField] private bool isGrounded;
    private float raycastLength = 2;
    public int damage = 1;

    private Animator animator;
    private Vector2 lastPosition;

    private enum enemyTypes
    { ground, flying }

    private enemyTypes currentEnemyType;

    public GameEvent onAttackEvent;
    private Knockback kb;
    [SerializeField] private bool isKnocked = false;
    [SerializeField] private float vulnerableTimer = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        playerPos = GameObject.FindGameObjectWithTag("Player");

        playerControls = FindAnyObjectByType<PrototypePlayerMovementControls>();

        animator = GetComponent<Animator>();

        kb = GetComponent<Knockback>();
    }

    private void Update()
    {
        if (!kb.IsBeingKnockedBack)
        {
            switch (currentEnemyType)
            {
                case enemyTypes.ground:
                    GroundEnemy();
                    break;

                case enemyTypes.flying:
                    FlyingEnemy();
                    break;
            }
        }
        VulnerableTimer();

        if (gameObject.tag == "GroundEnemy") { UpdateAnimation(); currentEnemyType = enemyTypes.ground; }
        if (gameObject.tag == "FlyingEnemy") { currentEnemyType = enemyTypes.flying; }
    }

    private void UpdateAnimation()
    {
        Vector2 currentPosition = transform.position;
        Vector2 velocity = (currentPosition - lastPosition) / Time.deltaTime;

        float horizontal = velocity.x;

        animator.SetFloat("horizontal", horizontal);

        lastPosition = currentPosition;

        if (horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (horizontal < 0)
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

        float jumpForce = 5;

        ////Jump
        //rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void FlyingEnemy()
    {
        rb2D.linearVelocity = Vector2.zero;
        float flyingEnemySpeed = GetComponent<BasicEnemyControls>().enemySpeed;
        transform.position = Vector2.MoveTowards(rb2D.position, playerPos.transform.position, flyingEnemySpeed * Time.deltaTime);
    }

    public void VulnerableTimer()
    {
        if (kb.IsBeingKnockedBack)
        {
            isKnocked = true;
            Debug.Log("has been knocked back");
        }
        else if (isKnocked)
        {
            vulnerableTimer -= Time.fixedDeltaTime;
            if (vulnerableTimer < 0)
            {
                vulnerableTimer = 1;
                isKnocked = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (vulnerableTimer < 1) return;

            //Raise a global event to attack player
            onAttackEvent.Raise(this, damage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (vulnerableTimer < 1) return;

            //Raise a global event to attack player
            onAttackEvent.Raise(this, damage);
        }
    }
}