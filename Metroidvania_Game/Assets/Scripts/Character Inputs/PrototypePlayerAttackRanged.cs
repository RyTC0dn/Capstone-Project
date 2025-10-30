using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypePlayerAttackRanged : MonoBehaviour
{
    [Header("Ranged Weapon Setup")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isAxeBought = false;


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
        if (Input.GetButtonDown("Fire2") && isAxeBought)
        {
            Shoot();
        }
    }

    public void AxeEvent(Component sender, object data)
    {
        if(data is bool boughtAxe)
        {
            if (boughtAxe)
            {
                isAxeBought = true;
            }           
        }
    }

    void Shoot()
    {
        Transform firingPoint = firePoint;
        animator.SetBool("isThrowing", true);
        //Instantiate ProjectilePrefab
        Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation);
        
        StartCoroutine(ResetWeapon());
    }

    IEnumerator ResetWeapon()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("isThrowing", false);
    }


}
