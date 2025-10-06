using UnityEngine;
using UnityEngine.InputSystem;

public class Shield : MonoBehaviour
{

    //public bool shieldIsUp;
    public Transform shieldPoint;
    public float sheildRange = 1.0f;
    public LayerMask attackLayer;

    //private void Awake()
    //{
    //    shieldIsUp = false;
    //}

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            ActivateShield();
        }
    }

    private void ActivateShield()
    {
        Collider2D[] blockAttack = Physics2D.OverlapCircleAll(shieldPoint.position, sheildRange, attackLayer);

        foreach (Collider2D attack in blockAttack)
        {
            Debug.Log("Blocked" +  attack.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(shieldPoint.position, sheildRange);
    }



}
