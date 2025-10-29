using System.Collections;
using UnityEngine;



public class BasicEnemyPatrolState : MonoBehaviour
{
    public float waitTime = 2.0f;
    private Vector3 destPos;

    BasicEnemyControls enemyControls;

    private bool isWaiting = false;

    public int currentWaypointIndex = 0; //Index of current waypoint
    public int positionOffsetX;
    public int positionOffsetY;
    private Vector3 startingPos;
    private Vector3[] waypoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyControls = GetComponent<BasicEnemyControls>();

        startingPos = transform.position;
    }

    private void SetPatrolPoints()
    {
        if (gameObject.CompareTag("GroundEnemy"))
        {
            
            waypoints = new Vector3[]
            {
                new Vector3(startingPos.x, startingPos.y),
                new Vector3(startingPos.x + positionOffsetX, startingPos.y + positionOffsetY),
            };

            Patrol(transform, waypoints[currentWaypointIndex], enemyControls.enemySpeed);
        }

        else if (gameObject.CompareTag("FlyingEnemy"))
        {
            waypoints = new Vector3[]
            {
                 new Vector3(startingPos.x, startingPos.y),
                 new Vector3(startingPos.x + positionOffsetX, startingPos.y + positionOffsetY),
            };

            Patrol(transform, waypoints[currentWaypointIndex], enemyControls.enemySpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetPatrolPoints();
    }

    public void Patrol(Transform currentPos, Vector3 waypoint, float speed)
    {
        destPos = waypoints[currentWaypointIndex];
        //waypoint = new Vector3(destPos.x, 0, 0);
        currentPos.position = Vector3.MoveTowards(currentPos.position, waypoint, speed * Time.deltaTime);

        
        Vector3 enemyPosX = new Vector3(currentPos.position.x, 0, 0);
        Vector3 waypointPosX = new Vector3(waypoint.x, 0, 0);

        Vector3 enemyPosY = new Vector3(0, currentPos.position.y, 0);
        Vector3 waypointPosY = new Vector3(0, waypoint.y, 0);

        if (gameObject.CompareTag("GroundEnemy"))//Check if the x position is within specific distance
        {
            if (Vector3.Distance(enemyPosX, waypointPosX) < 1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
        else if (gameObject.CompareTag("FlyingEnemy"))//Check if the flying enemies 
        {
            if (Vector3.Distance(enemyPosY, waypointPosY) < 1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
        
    }
}
