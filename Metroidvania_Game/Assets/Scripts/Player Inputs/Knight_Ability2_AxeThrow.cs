using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Knight_Ability2_AxeThrow : MonoBehaviour
{
    [Header("Ranged Weapon Setup")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private bool axeSelected;

    [Header("Axe Settings")]
    public float throwRate = 1f;
    private float delayTillThrow;
    [SerializeField]private float throwCooldown = 0;
    private bool wasThrown = false;



    PrototypePlayerMovementControls playerController;
    PrototypeShop shop;
    public SceneInfo sceneInfo;
    [SerializeField]private GameObject axeIcon;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PrototypePlayerMovementControls>();
        shop = FindAnyObjectByType<PrototypeShop>();
        delayTillThrow = throwRate;

        axeIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //AxeEvent(shop, isAxeBought);
        if (Input.GetButtonDown("Fire2") && axeSelected)
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

        if (axeSelected)
            axeIcon.SetActive(true);
        else
            axeIcon.SetActive(false);
    }

    public void OnButtonSelected()
    {
        axeSelected = true;
    }

    public void OnButtonDeselect()
    {
        axeSelected = false;
    }

    void Shoot()
    {
        if(throwCooldown <= 0)
        {
            Transform firingPoint = firePoint;
            animator.Play("KnightAxeThrow");
            //Instantiate ProjectilePrefab
            Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation);
            throwCooldown = 1f / delayTillThrow;
            wasThrown = true;

            StartCoroutine(ResetWeapon());
        }
               
    }

    IEnumerator ResetWeapon()
    {
        yield return new WaitForSeconds(animator.recorderStopTime);
        // Reset animator to initial state
        animator.SetBool("isThrowing", false);
    }


}
