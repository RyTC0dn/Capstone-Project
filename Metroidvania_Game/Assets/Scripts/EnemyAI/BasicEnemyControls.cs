using System.Collections.Generic;
using UnityEngine;

public enum States
{
    Patrol, 
    Attack, 
}

public class BasicEnemyControls : MonoBehaviour
{
    [Header ("Enemy States")]
    BasicEnemyPatrolState enemyPatrolState;
    BasicEnemyAttackState attackState;

    public float enemySpeed;
    private Rigidbody2D enemyRB2D;

    private bool isAtWaypoint = false;

   

    //public Dictionary<Transform, int> patrolWaypoints = new Dictionary<Transform, int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyPatrolState = GetComponent<BasicEnemyPatrolState>();
        enemyRB2D = GetComponent<Rigidbody2D>();
        attackState = GetComponent<BasicEnemyAttackState>();


        //patrolWaypoints.Add(waypoints[currentWaypointIndex], currentWaypointIndex);
    }

    // Update is called once per frame
    void Update()
    {
        //enemyPatrolState.Patrol(gameObject.transform, waypoints[currentWaypointIndex].position, enemySpeed);
        //attackState.AttackPlayer(enemyRB2D, enemySpeed);
    }
}
