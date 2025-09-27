using System.Collections;
using UnityEngine;



public class BasicEnemyPatrolState : MonoBehaviour
{
    public float waitTime = 2.0f;
    private Vector3 destPos;

    BasicEnemyControls enemyControls;

    private bool isWaiting = false;

    public int currentWaypointIndex = 0; //Index of current waypoint
    public Transform[] waypoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyControls = GetComponent<BasicEnemyControls>();       
    }

    // Update is called once per frame
    void Update()
    {
        Patrol(transform, waypoints[currentWaypointIndex].position, enemyControls.enemySpeed);
    }

    public void Patrol(Transform currentPos, Vector3 waypoint, float speed)
    {
        destPos = waypoints[currentWaypointIndex].position;
        //waypoint = new Vector3(destPos.x, 0, 0);
        currentPos.position = Vector3.MoveTowards(currentPos.position, waypoint, speed * Time.deltaTime);

        Vector3 enemyPos = new Vector3(currentPos.position.x, 0, 0);
        Vector3 waypointPos = new Vector3(waypoint.x, 0, 0);

        if (Vector3.Distance(enemyPos, waypointPos) < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
