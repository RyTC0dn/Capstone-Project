using UnityEngine;

public class BasicEnemyControls : MonoBehaviour
{
    BasicEnemyPatrolState enemyPatrolState;

    public Transform[] waypoints;

    [SerializeField]
    private float enemySpeed;
    private Rigidbody2D enemyRB2D;

    private bool isAtWaypoint = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyPatrolState = FindAnyObjectByType<BasicEnemyPatrolState>();
        enemyRB2D = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyPatrolState.Patrol(gameObject.transform, enemySpeed);
    }
}
