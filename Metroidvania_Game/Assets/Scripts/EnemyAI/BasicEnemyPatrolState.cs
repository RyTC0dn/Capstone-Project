using System.Collections;
using UnityEngine;

public class BasicEnemyPatrolState : MonoBehaviour
{
    private Transform[] waypoints; //Array to hold multiple waypoints
    private int currentWaypointIndex = 0; //Indec of current waypoint
    public float waitTime = 2.0f;
    private Vector3 destPos;

    private bool isWaiting = false; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int randomWaypoint = Random.Range(0, waypoints.Length);
        destPos = waypoints[randomWaypoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Patrol(Transform currentPos, float speed)
    {
        if (waypoints.Length == 0)
            return;


        Vector3 waypointPos = new Vector3(destPos.x, 0, 0);
        currentPos.position = Vector3.MoveTowards(currentPos.position, waypointPos, speed * Time.deltaTime);

        if(Vector3.Distance(currentPos.position, waypointPos) > 0.2f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }
}
