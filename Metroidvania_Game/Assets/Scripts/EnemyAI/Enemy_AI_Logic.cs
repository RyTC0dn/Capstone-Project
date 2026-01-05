using Unity.Behavior;
using UnityEngine;

public class Enemy_AI_Logic : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;
    [Space(20)]

    [Header("Patrol Settings")]
    [SerializeField]private float enemySpeed;
    [SerializeField]private GameObject[] currentWaypoint;
    private int waypointIndex;
    private bool patrolling = true;

    [Header("Combat Settings")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField]private float attackCooldown = 1f;
    public GameEvent onAttackEvent;
    [Space(20)]

    [Header("Detection Ranges")]
    [SerializeField]private float visionRange;
    [SerializeField] private float fovAngle;
    [SerializeField]private float attackRange;
    private bool isPlayerInSight;
    private bool isPlayerInRange = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (patrolling && !isPlayerInSight || patrolling && !isPlayerInRange)
        {
            Patrolling();
        }

        DetectPlayer();
    }

    private void FixedUpdate()
    {
        
    }

    private void Patrolling()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentWaypoint[waypointIndex].transform.position, 
            enemySpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, currentWaypoint[waypointIndex].transform.position) < 0.2f)
        {
            //go to next waypoint
            waypointIndex++;

            if(waypointIndex >= currentWaypoint.Length)
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
        //Could also use other actions here
        onAttackEvent.Raise(this, attackDamage);
    }

    private void DetectPlayer()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.right, visionRange);
        if(ray.collider != null)
        {
            isPlayerInSight = ray.collider.CompareTag("Player");
            Vector3 end = new Vector3(transform.position.x - visionRange, transform.position.y);
            Debug.DrawRay(transform.position, transform.position - end, Color.green);
            if (isPlayerInSight)
            {
                Debug.DrawRay(transform.position, Vector2.left, Color.green);
                Debug.Log("Sees player");
            }
        }
    }

    #region AttackActions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }
    #endregion
}
