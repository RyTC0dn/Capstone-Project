using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float enemySpeed;
    [SerializeField]private float jumpHeight; //If the enemy is caught in a pit that they can jump out of
    public float detectorRange;
    [Space(20)]

    [Header("Waypoints Assignment")]
    [SerializeField] private GameObject[] waypoints;
    private int waypointIndex;

    private bool playerDetected;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PatrolFunction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PatrolFunction()
    {
        if(Vector2.Distance(gameObject.transform.position, waypoints[waypointIndex].transform.position) > 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, enemySpeed * Time.deltaTime);
        }
        else
        {
            waypointIndex++;
        }
    }
}
