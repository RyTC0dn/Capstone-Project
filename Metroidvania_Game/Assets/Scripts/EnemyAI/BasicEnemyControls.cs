using System.Collections.Generic;
using UnityEngine;

public enum States
{
    Patrol, 
    Attack, 
    Death
}

public class BasicEnemyControls : MonoBehaviour
{
    BasicEnemyPatrolState enemyPatrolState;
    public int currentWaypointIndex = 0; //Index of current waypoint
    public Transform[] waypoints;

    [SerializeField]
    private float enemySpeed;
    private Rigidbody2D enemyRB2D;

    private bool isAtWaypoint = false;

    public Dictionary<Transform, int> patrolWaypoints = new Dictionary<Transform, int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyPatrolState = GetComponent<BasicEnemyPatrolState>();
        enemyRB2D = GetComponent<Rigidbody2D>();

        patrolWaypoints.Add(waypoints[currentWaypointIndex], currentWaypointIndex);
    }

    // Update is called once per frame
    void Update()
    {
        enemyPatrolState.Patrol(gameObject.transform, enemySpeed);
    }
}
