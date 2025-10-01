using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypePlayerAttackRanged : MonoBehaviour
{
    [Header("Ranged Weapon Setup")]
    public Transform firepointPosRight;
    public Transform firepointPosLeft;
    public GameObject projectilePrefab;



    PrototypePlayerMovementControls playerController;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PrototypePlayerMovementControls>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //Instantiate ProjectilePrefab
        Instantiate(projectilePrefab,firepointPosRight.position, firepointPosRight.rotation);

    }
}
