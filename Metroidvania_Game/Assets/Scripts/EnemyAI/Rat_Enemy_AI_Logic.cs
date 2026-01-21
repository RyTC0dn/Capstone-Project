using NUnit.Framework;
using System.Collections;
using TMPro;
using Unity.AppUI.UI;
using UnityEngine;

public enum RatState
{
    Patrol,
    Chase,
    Attack
}

/// <summary>
/// This script is to lay the foundations of the AI functions
/// </summary>
public class Rat_Enemy_AI_Logic : MonoBehaviour
{
    [Header("References")]
    private Transform playerTransform;
    private Collider2D col;
    private Rigidbody2D rb2D;
    private Animator animator;
    private SpriteRenderer ratSp;
    private float horizontal;
    private float h;
    private RatState currentState;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstructLayer;
    [Space(20)]

    [Header("Patrol Settings")]
    [SerializeField] private float enemySpeed;
    [SerializeField] private GameObject[] currentWaypoint;
    [SerializeField] private Vector2[] waypoints;
    [SerializeField] private int waypointIndex;
    private bool patrolling = true;
    [Tooltip("Manually assign to keep enemy on ledge or not to walk off")]
    [SerializeField] private bool stayOnLedge; 

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
    private bool playerInView = false;
    private bool playerInRange = false;
    [UnityEngine.Range(0, 1)]
    public float detectorTransparency;

    [Tooltip("Should be the same as the death animation run time")]
    [SerializeField] private float chaseTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        ratSp = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        attackCollider.enabled = false;

        currentDirection = startingDirection;
        ratSp.flipX = true;
        currentState = RatState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        ////Detection logic functions
        //DetectPlayer();
        animator.applyRootMotion = false;
    }

    public void StateSwitch(RatState state)
    {
        if(currentState == state) return;

        currentState = state;
    }

    private void FixedUpdate()
    {
        if (IfIsInKnockBack)
            return;

        if(rb2D.linearVelocity.x > 0)
        {
            ratSp.flipX = true;
        }
        else if (rb2D.linearVelocity.x < 0)
        {
            ratSp.flipX = false;
        }

        switch (currentState)
        {
            case RatState.Patrol:
                Patrolling();

                if(playerInView)
                    StateSwitch(RatState.Chase);
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

    public void KnockedBack(bool state)
    {
        IfIsInKnockBack = state;
    }

    private bool IsGroundAhead()
    {
        Bounds b = col.bounds;

        Vector2 downOrigin = currentDirection > 0
            ? new Vector2(b.max.x, b.min.y)
            : new Vector2(b.min.x, b.min.y);

        return Physics2D.Raycast(
            downOrigin,
            Vector2.down,
            0.15f,
            LayerMask.GetMask("Ground")
        );
    }

    private void Patrolling()
    {
        if (IfIsInKnockBack)
        {
            return;
        }

        #region Patrolling
        rb2D.linearVelocity = new Vector2(enemySpeed * currentDirection, rb2D.linearVelocity.y);

        Bounds b = col.bounds;

        //Ray origins
        Vector2 facingOrigin = currentDirection > 0 
            ? new Vector2(b.max.x + 0.05f, b.center.y) 
            : new Vector2(b.min.x - 0.05f, b.center.y);

        Vector2 downOrigin = currentDirection > 0 
            ? new Vector2(b.max.x, b.min.y) 
            : new Vector2(b.min.x, b.min.y);

        //Wall and ground checking 
        bool wallHit = Physics2D.Raycast(facingOrigin, Vector2.right * currentDirection, 
            0.1f, LayerMask.GetMask("Ground"));

        bool groundAhead = Physics2D.Raycast(downOrigin, Vector2.down, 
            0.15f, LayerMask.GetMask("Ground"));

        Debug.DrawRay(facingOrigin, Vector2.right * currentDirection * 0.1f, Color.red);
        Debug.DrawRay(downOrigin, Vector2.down * 0.15f, Color.blue);

        if(wallHit || !groundAhead && stayOnLedge)
        {
            Flip();
        }
        #endregion

        #region Check if Player is in view
        bool playerHit = Physics2D.Raycast(facingOrigin, Vector2.right * currentDirection,
            visionRange, LayerMask.GetMask("Player"));

        if(playerHit && patrolling)
        {
            playerInView = true;
        }
        #endregion
    }

    private void Flip()
    {
        //Flip sprite
        currentDirection *= -1;
    }

    private void Chase()
    {
        if (IfIsInKnockBack)
            return;

        float directionToPlayer =
            playerTransform.position.x > transform.position.x ? 1f : -1f;

        currentDirection = (int)directionToPlayer;
        ratSp.flipX = currentDirection < 0;

        // Stop chasing if no ground ahead
        if (!IsGroundAhead())
        {
            rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
            return;
        }

        rb2D.linearVelocity = new Vector2(
            enemySpeed * currentDirection,
            rb2D.linearVelocity.y
        );

        float distance = Vector2.Distance(rb2D.position, playerTransform.position);

        if (distance <= attackRange)
        {
            rb2D.linearVelocity = Vector2.zero;
            currentState = RatState.Attack;
        }
    }

    private void Attack()
    {
        if (IfIsInKnockBack)
            return;

        if (isAttacking)
            return;

        isAttacking = true;

        animator.Play("EnemyRatAttack");
        
        float direction = playerTransform.position.x > transform.position.x ? 1f : -1f;

        Vector2 launchVector = new Vector2(direction * launchForceX, launchForceY);

        StartCoroutine(ApplyLaunchForce(launchVector));
    }

    private IEnumerator ApplyLaunchForce(Vector2 force)
    {
        yield return new WaitForFixedUpdate();
        rb2D.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;

        StateSwitch(RatState.Chase);
    }

    public void DeathState(Object enemy)
    {
        animator.Play("EnemyRatDeath");

        //Could also turn off the sprite and add particle effects before destroy
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
    }
}
