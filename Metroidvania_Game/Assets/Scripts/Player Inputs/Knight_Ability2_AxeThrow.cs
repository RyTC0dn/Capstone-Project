using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Knight_Ability2_AxeThrow : MonoBehaviour
{
    [Header("Ranged Weapon Setup")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isAxeBought;

    [Header("Axe Settings")]
    public float throwRate = 1f;
    private float delayTillThrow;
    [SerializeField]private float throwCooldown = 0;
    private bool wasThrown = false;



    PrototypePlayerMovementControls playerController;
    PrototypeShop shop;
    public SceneInfo sceneInfo;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PrototypePlayerMovementControls>();
        shop = FindAnyObjectByType<PrototypeShop>();
        isAxeBought = PlayerPrefs.GetInt("AxeBought", 0) == 1;
        delayTillThrow = throwRate;
    }

    // Update is called once per frame
    void Update()
    {
        //AxeEvent(shop, isAxeBought);
        if (Input.GetButtonDown("Fire2") && sceneInfo.isAxeBought)
        {
            Shoot();
        }
        if(wasThrown)
        {
            throwCooldown -= Time.deltaTime;
            if(throwCooldown <= 0)
            {
                wasThrown = false;
            }
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
        if(throwCooldown <= 0)
        {
            Transform firingPoint = firePoint;
            animator.SetBool("isThrowing", true);
            //Instantiate ProjectilePrefab
            Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation);
            throwCooldown = 1f / delayTillThrow;
            wasThrown = true;

            StartCoroutine(ResetWeapon());
        }
               
    }

    IEnumerator ResetWeapon()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("isThrowing", false);
    }


}
