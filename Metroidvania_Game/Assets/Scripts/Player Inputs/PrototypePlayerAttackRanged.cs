using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypePlayerAttackRanged : MonoBehaviour
{
    [Header("Ranged Weapon Setup")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isAxeBought;


    PrototypePlayerMovementControls playerController;
    PrototypeShop shop;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PrototypePlayerMovementControls>();
        shop = FindAnyObjectByType<PrototypeShop>();
        isAxeBought = PlayerPrefs.GetInt("AxeBought", 0) == 1;
    }

    // Update is called once per frame
    void Update()
    {
        AxeEvent(shop, isAxeBought);
        if (Input.GetButtonDown("Fire2") && isAxeBought)
        {
            Shoot();
        }
    }

    public void AxeEvent(Component sender, object data)
    {
        if(data is bool)
        {
            bool bought = (bool)data;
            if (bought)
            {
                isAxeBought=true;
            }
        }
        else
        {
            Debug.LogWarning("AxeEvent called but data is not a bool");
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
