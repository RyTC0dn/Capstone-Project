using System.Xml.Serialization;
using Unity.Behavior;
using UnityEngine;

public class Enemy_AI_Logic : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    private Vector2 lastPos;
    private float horizontal;
    public static Enemy_AI_Logic Instance { get {  return Instance; } }

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
        DetectPlayer();

        if (patrolling && !isPlayerInSight || patrolling && !isPlayerInRange)
        {
            Patrolling();
        }
        else if (!patrolling && isPlayerInSight)
        {
            ChasePlayer();
        }
    }

    private void FixedUpdate()
    {
        Vector2 currentPosition = transform.position;
        Vector2 velocity = (currentPosition - lastPos) / Time.deltaTime;

        horizontal = velocity.x;

        lastPos = currentPosition;

        if(horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if(horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
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
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.left, 
            visionRange, playerLayer);
        if(ray.collider != null)
        {
            Debug.DrawRay(transform.position, Vector2.left * visionRange, Color.green);
            Debug.Log("Sees player");
            isPlayerInSight = true;
            patrolling = false;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.left * visionRange, Color.red);
            Debug.Log("Sees player");
            isPlayerInSight = false;
            patrolling = true;
        }
    }

    #region AttackActions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }
    #endregion
}
