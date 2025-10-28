using System.Collections;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [Header("Turret Setup")]
    public GameObject bullet;
    public Transform firingPoint;
    public float rateOfFire = 1f;
    private float firingRate;

    private float firingCooldown = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firingRate = rateOfFire;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if(firingCooldown <= 0)
        {
            Instantiate(bullet, firingPoint.transform.position, Quaternion.identity);
            firingCooldown = 1f / firingRate;
        }
        firingCooldown -= Time.deltaTime;        
    }
}
