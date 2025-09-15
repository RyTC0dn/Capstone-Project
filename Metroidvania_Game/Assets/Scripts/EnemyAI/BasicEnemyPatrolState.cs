using System.Collections;
using UnityEngine;

public class BasicEnemyPatrolState : MonoBehaviour
{
    [SerializeField]
    private int currentWaypointIndex = 0; //Indec of current waypoint
    public float waitTime = 2.0f;
    private Vector3 destPos;

    BasicEnemyControls enemyControls;

    private bool isWaiting = false; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyControls = FindAnyObjectByType<BasicEnemyControls>();
        int randomWaypoint = Random.Range(0, enemyControls.waypoints.Length);
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Patrol(Transform currentPos, float speed)
    {
        destPos = enemyControls.waypoints[currentWaypointIndex].position;
        Vector3 waypointPos = new Vector3(destPos.x, 0, 0);
        currentPos.position = Vector3.MoveTowards(currentPos.position, waypointPos, speed * Time.deltaTime);

        if (Vector3.Distance(currentPos.position, waypointPos) < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % enemyControls.waypoints.Length;
        }
    }
}
