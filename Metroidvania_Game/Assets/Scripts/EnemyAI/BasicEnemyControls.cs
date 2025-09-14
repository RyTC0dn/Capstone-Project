using UnityEngine;

public class BasicEnemyControls : MonoBehaviour
{
    BasicEnemyPatrolState enemyPatrolState;

    public Transform[] waypoints;

    [SerializeField]
    private float enemySpeed;
    private Rigidbody2D enemyRB2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyPatrolState = FindAnyObjectByType<BasicEnemyPatrolState>();
        enemyRB2D = GetComponent<Rigidbody2D>();
        enemyPatrolState.Patrol(transform, enemySpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
