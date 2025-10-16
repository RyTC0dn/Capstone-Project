using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypePlayerAttackRanged : MonoBehaviour
{
    [Header("Ranged Weapon Setup")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    [SerializeField] private Animator animator;


    PrototypePlayerMovementControls playerController;
    PrototypeShop shop;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PrototypePlayerMovementControls>();
        shop = FindAnyObjectByType<PrototypeShop>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") /*&& shop.boughtAxe == true*/)
        {
            Shoot();
            //animator.SetBool("isThrowing", true);

        }

    }

    void Shoot()
    {
        Transform firingPoint = firePoint;

        //Instantiate ProjectilePrefab
        Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation);

    }


}
