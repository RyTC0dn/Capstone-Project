using NUnit.Framework;
using System.Collections;
using Unity.AppUI.UI;
using UnityEngine;

public enum RatState
{
    Patrol,
    Chase,
    Attack
}

public class Rat_Enemy_AI_Logic : MonoBehaviour
{
    [Header("References")]
    private Transform playerTransform;
    private Rigidbody2D rb2D;
    private Animator animator;
    private SpriteRenderer ratSp;
    private Vector2 lastPos;
    private Vector2 movement;
    private float horizontal;
    private float h;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstructLayer;
    [Space(20)]

    [Header("Patrol Settings")]
    [SerializeField] private float enemySpeed;
    [SerializeField] private GameObject[] currentWaypoint;
    [SerializeField] private Vector2[] waypoints;
    [SerializeField] private int waypointIndex;
    [SerializeField] private float stopTimer; //For how long an enemy stays at a particular position
    private bool patrolling = true;
    private bool isGrounded;

    [SerializeField] private int startingDirection = 1;
    private int currentDirection;
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
    public bool IfIsInKnockBack { get; private set; }
    [Space(20)]

    [Header("Detection Ranges")]
    [SerializeField] private float visionRange; //How far the AI can see 
    [SerializeField] private float fovAngle; //The vision angle 
    [SerializeField] private float attackRange;
    private float halfWidth;
    private float halfHeight;
    private bool isPlayerInSight = false;
    private bool isPlayerInRange = false;
    [UnityEngine.Range(0, 1)]
    public float detectorTransparency;

    [Tooltip("Should be the same as the death animation run time")]
    [SerializeField] private float deathTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        ratSp = GetComponent<SpriteRenderer>();
        attackCollider.enabled = false;

        //Set the width and height of the rat enemy sprite
        halfWidth = ratSp.bounds.extents.x;
        halfHeight = ratSp.bounds.extents.y;
        currentDirection = startingDirection;
    }

    // Update is called once per frame
    void Update()
    {
        //Detection logic functions
        DetectPlayer();

        animator.applyRootMotion = false;
    }

    public void StateSwitch(RatState state)
    {

        switch (state)
        {
            case RatState.Patrol:
                Patrolling();
                break;
            case RatState.Chase:
                Chase();
                break;
            case RatState.Attack:
                Attack();
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        //movement.x = enemySpeed * currentDirection;
        //movement.y = rb2D.linearVelocity.y;
        //rb2D.linearVelocity = movement;

        CheckDistanceToPlayer();

        if (IfIsInKnockBack)
            return;

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

    public void KnockedBack(bool state)
    {
        IfIsInKnockBack = state;
    }

    private void Patrolling()
    {
        //Vector2 rightPos = transform.position;
        //Vector2 leftPos = transform.position;
        //rightPos.x += halfWidth;
        //leftPos.x -= halfWidth;

        //Debug.DrawRay(transform.position, Vector2.right * (halfWidth + 0.1f), Color.red);
        //Debug.DrawRay(transform.position, Vector2.left * (halfWidth + 0.1f), Color.red);
        //if (rb2D.linearVelocity.x > 0)
        //{

        //    if (Physics2D.Raycast(transform.position, Vector2.right, halfWidth + 0.1f, LayerMask.GetMask("Ground"))
        //        && rb2D.linearVelocity.x > 0)
        //    {
        //        //Draw raycast from rat to the right 
        //        //Check if the rat enemy is moving right

        //        currentDirection *= -1;
        //        ratSp.flipX = false;
        //    }
        //    else if (!Physics2D.Raycast(rightPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground")))
        //    {
        //        currentDirection *= -1;
        //        ratSp.flipX = false;
        //    }
        //}
        //else if (rb2D.linearVelocity.x < 0)
        //{
        //    if (Physics2D.Raycast(transform.position, Vector2.left, halfWidth + 0.1f, LayerMask.GetMask("Ground"))
        //    && rb2D.linearVelocity.x < 0)
        //    {
        //        currentDirection *= -1;
        //        ratSp.flipX = true;
        //    }
        //    else if (!Physics2D.Raycast(leftPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground")))
        //    {
        //        currentDirection *= -1;
        //        ratSp.flipX = true;
        //    }
        //}


        if (IfIsInKnockBack)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex],
            enemySpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, waypoints[waypointIndex]) < 0.2f)
        {
            stopTimer--;

            if (stopTimer <= 0)
            {
                //go to next waypoint
                waypointIndex++;
                stopTimer = Random.Range(0, 10);
            }

            if (waypointIndex >= waypoints.Length)
            {
                //Reset waypoint index once no more waypoints found
                waypointIndex = 0;
            }
        }
    }

    private void Chase()
    {
        if (IfIsInKnockBack) { return; }

        transform.position = Vector2.MoveTowards(transform.position,
            playerTransform.position, enemySpeed * Time.deltaTime);
    }

    private void Attack()
    {
        if (IfIsInKnockBack)
            return;

        if (!canAttack || isAttacking)
            return;

        isAttacking = true;
        canAttack = false;

        animator.Play("EnemyRatAttack");

        float direction = playerTransform.position.x > transform.position.x ? 1f : -1f;

        Vector2 launchVector = new Vector2(launchForceX * direction, launchForceY);

        StartCoroutine(ApplyLaunchForce(launchVector));
    }

    private IEnumerator ApplyLaunchForce(Vector2 force)
    {
        yield return new WaitForFixedUpdate();
        rb2D.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        canAttack = true;
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
        Physics2D.OverlapCircleAll(transform.position, visionRange / 2, playerLayer);

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
            StateSwitch(RatState.Chase);
        }
        else
        {
            StateSwitch(RatState.Patrol);
        }
        #endregion
    }

    private void CheckDistanceToPlayer()
    {
        #region Attack Transition
        if (Vector2.Distance(transform.position,
            playerTransform.position) <= attackRange)
        {
            StateSwitch(RatState.Attack);
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

        Gizmos.color = Color.yellow;
        foreach (Vector2 pos in waypoints)
        {
            Gizmos.DrawSphere((Vector3)pos, 2f / 2);
        }

        Color circle = new Color(0, 255, 0, 0.10f);
        Gizmos.color = circle;
        Gizmos.DrawSphere(transform.position, visionRange / 2);
    }
}
