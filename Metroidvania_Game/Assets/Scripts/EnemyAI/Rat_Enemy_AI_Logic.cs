using Unity.AppUI.UI;
using UnityEngine;

public class Rat_Enemy_AI_Logic : MonoBehaviour
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
    public GameEvent onAttackEvent;
    public Collider2D attackCollider;
    [SerializeField] private float launchDistance;
    [SerializeField] private float launchHeight;
    private bool canAttack = false;
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

        //State logic functions
        if (patrolling && !isPlayerInSight)
        {
            Patrolling();
        }
        else if (!patrolling && isPlayerInSight)
        {
            ChasePlayer();
        }
        else if (!patrolling && isPlayerInRange)
        {
            AttackPlayer();
        }
    }

    private void FixedUpdate()
    {
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

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, enemySpeed * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        attackCollider.enabled = true;

        canAttack = true;

        animator.Play("EnemyRatAttack");

        Vector2 launchAtk = new Vector2(launchDistance, launchHeight);

        Vector2 distanceToPlayer = (playerTransform.position - transform.position).normalized;


        rb2D.AddForce(distanceToPlayer * launchDistance, ForceMode2D.Impulse);

        if (attackCollider.CompareTag("Player"))
        {
            //Could also use other actions here
            onAttackEvent.Raise(this, attackDamage);
            Debug.Log("Enemy attacked");
        }
    }

    public void AttackRange()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
            isPlayerInRange = true;
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


        if (topRay != null)
        {
            playerDetected = true;
        }
        else if (bottomRay != null)
        {
            playerDetected = true;
        }

        if (playerDetected)
        {
            Debug.Log("Player Detetced");
            patrolling = false;
            isPlayerInSight = true;
        }
        else
        {
            isPlayerInSight = false;
            patrolling = true;
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

    #region AttackActions
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        AttackPlayer();
    //    }
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        AttackPlayer();
    //    }
    //}
    #endregion
}
