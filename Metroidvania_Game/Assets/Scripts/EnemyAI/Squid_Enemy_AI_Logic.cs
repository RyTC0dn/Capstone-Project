using UnityEngine;

public class Squid_Enemy_AI_Logic : MonoBehaviour
{
    [Header("References")]
    private Transform playerTransform;
    private Rigidbody2D rb2D;
    private Animator animator;
    private Vector2 lastPos;
    private float horizontal;
    private float h;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstructLayer;
    [Space(20)]

    [Header("Patrol Settings")]
    [SerializeField] private float enemySpeed;
    [SerializeField] private GameObject[] currentWaypoint;
    private int waypointIndex;
    private bool patrolling = true;
    [Space(20)]

    [Header("Combat Settings")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float launchForceX = 6f;
    [SerializeField] private float launchForceY = 4f;
    public GameEvent onAttackEvent;
    public Collider2D attackCollider;

    private bool canAttack = true;
    private bool isAttacking = false;
    [Space(20)]

    [Header("Detection Ranges")]
    [SerializeField] private float visionRange; //How far the AI can see 
    [SerializeField] private float fovAngle; //The vision angle 
    [SerializeField] private float attackRange;
    private bool isPlayerInSight = false;
    private bool isPlayerInRange = false;
    [Range(0, 1)]
    public float detectorTransparency;

    [Tooltip("Should be the same as the death animation run time")]
    [SerializeField] private float deathTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        attackCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Detection logic functions
        DetectPlayer();
    }

    public void StateSwitch(CurrentState state)
    {
        if (isAttacking && state != CurrentState.Attack)
            return;

        switch (state)
        {
            case CurrentState.Patrol:
                Patrolling();
                break;
            case CurrentState.Chase:
                Chase();
                break;
            case CurrentState.Attack:
                Attack();
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        CheckDistanceToPlayer();

        Vector2 currentPosition = transform.position;
        Vector2 velocity = (currentPosition - lastPos) / Time.deltaTime;

        horizontal = velocity.x;

        lastPos = currentPosition;
        if (horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void Patrolling()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentWaypoint[waypointIndex].transform.position,
             enemySpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentWaypoint[waypointIndex].transform.position) < 0.2f)
        {
            //go to next waypoint
            waypointIndex++;

            if (waypointIndex >= currentWaypoint.Length)
            {
                //Reset waypoint index once no more waypoints found
                waypointIndex = 0;
            }
        }
    }

    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            playerTransform.position, enemySpeed * Time.deltaTime);
    }

    private void Attack()
    {
        if (!canAttack || isAttacking)
            return;

        isAttacking = true;
        canAttack = false;

        animator.Play("EnemyRatAttack");

        float direction = playerTransform.position.x > transform.position.x ? 1f : -1f;

        Vector2 launchVector = new Vector2(direction * launchForceX, launchForceY);
    }

    private void DetectPlayer()
    {
        if (horizontal > 0)
        {
            h = -1;
        }
        else if (horizontal < 0)
        {
            h = 1;
        }

        #region Detection Logic
        Vector2 direction = h > 0 ? Vector2.left : Vector2.right;
        bool playerDetected = false;

        Vector2 topDir = Quaternion.Euler(0, 0, fovAngle * 0.5f) * direction;
        Vector2 bottomDir = Quaternion.Euler(0, 0, -fovAngle * 0.5f) * direction;

        RaycastHit2D topRay = Physics2D.Raycast(transform.position, topDir, visionRange, playerLayer);
        RaycastHit2D bottomRay = Physics2D.Raycast(transform.position, bottomDir, visionRange, playerLayer);

        Debug.DrawRay(transform.position, topDir * visionRange, Color.yellow);
        Debug.DrawRay(transform.position, bottomDir * visionRange, Color.yellow);


        if (topRay == playerTransform)
        {
            playerDetected = true;
        }
        else if (bottomRay == playerTransform)
        {
            playerDetected = true;
        }

        if (playerDetected)
        {
            StateSwitch(CurrentState.Chase);
        }
        else
        {
            StateSwitch(CurrentState.Patrol);
        }
        #endregion
    }

    private void CheckDistanceToPlayer()
    {
        #region Attack Transition
        if (Vector2.Distance(transform.position,
            playerTransform.position) <= attackRange)
        {
            StateSwitch(CurrentState.Attack);
        }
        #endregion
    }

    public void DeathState(Object enemy)
    {
        animator.Play("EnemyRatDeath");

        //Could also turn off the sprite and add particle effects before destroy

        Destroy(enemy, deathTimer);
    }

    private void OnDrawGizmos()
    {
        Color detection = new Color(255, 0, 0, detectorTransparency);

        Gizmos.color = detection;
        Gizmos.DrawSphere(transform.position, attackRange / 2);
    }
}
