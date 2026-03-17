using UnityEngine;

public class SpitFirepoint : MonoBehaviour
{
    private GameObject target;
    private float rotationSpeed = 2.5f;
    private Vector3 lastKnownPosition;
    public Quaternion targetRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //Find the player and set the last known position to the player's current position
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        lastKnownPosition = new Vector3(target.transform.position.x,
            target.transform.position.y + 1, target.transform.position.z);
        AimDirection(lastKnownPosition);
    }

    private void AimDirection(Vector3 lastPos)
    {
        Vector3 direction = (lastPos - transform.position).normalized;
        targetRot = Quaternion.FromToRotation(transform.right, direction) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }
}