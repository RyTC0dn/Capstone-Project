using UnityEngine;

public class EnemyPatrolState : IState
{
    public Transform[] waypoints; //Array of waypoints
    private int currentWaypointIndex; //Current waypoint index

    public void OnEnter()
    {
       
    }

    public void UpdateState()
    {

    }

    public void OnHurt()
    {

    }

    public void OnExit()
    {

    }
}
