using System.Collections.Generic;
using UnityEngine;

public enum States
{
    None,
    Patrol, 
    Attack, 
}

public class BasicEnemyControls : MonoBehaviour
{
    [Header ("Enemy States")]
    BasicEnemyPatrolState enemyPatrolState;
    BasicEnemyAttackState attackState;
    public States currentEnemyState;

    [Header("Enemy Setup")]
    public float enemySpeed;
    public float playerDistance;
    private Rigidbody2D enemyRB2D;
    public int enemyHealth = 2;

    //public Dictionary<Transform, int> patrolWaypoints = new Dictionary<Transform, int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyPatrolState = GetComponent<BasicEnemyPatrolState>();
        enemyRB2D = GetComponent<Rigidbody2D>();
        attackState = GetComponent<BasicEnemyAttackState>();
    }

    // Update is called once per frame
    void Update()
    {
        StateSwitch();
    }

    public void StateSwitch()
    {
        switch (currentEnemyState)
        {
            case States.Attack:
                attackState.enabled = true;
                enemyPatrolState.enabled = false;
                break;
            case States.Patrol:
                enemyPatrolState.enabled = true;
                attackState.enabled = false;
                break;
            case States.None:
                enemyPatrolState.enabled = false;
                attackState.enabled = false;
                break;
        }        

        ////Setting up the conditions for state change
        //Vector2 playerVector = new Vector2(playerControls.transform.position.x, playerControls.transform.position.y);
        //Vector2 enemyVector = new Vector2(transform.position.x, transform.position.y);
  
        //if(Vector2.Distance(enemyVector, playerVector) <= playerDistance) //If the distance between enemy and player is less than 0.5
        //{
        //    currentEnemyState = States.Attack;
        //    Debug.Log("Attack State");
        //}
        //else
        //{
        //    currentEnemyState= States.Patrol;
        //    Debug.Log("Patrol State");
        //}
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector2.down * playerDistance, Color.blue);
    }
}
