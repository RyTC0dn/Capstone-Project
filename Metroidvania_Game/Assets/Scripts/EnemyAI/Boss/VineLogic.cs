using UnityEngine;

public enum VineType
{
    Spike,
    Grab,
    Hazard
}

public class VineLogic : MonoBehaviour
{
    private int damage = 1;
    public Animator animator;
    private PlayerHealth playerHP;

    [SerializeField]
    private float lifeTime = 2.0f;

    private float currentLifeTime;

    public VineType vineType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerHP = FindFirstObjectByType<PlayerHealth>();
        currentLifeTime = lifeTime;
    }

    // Update is called once per frame
    private void Update()
    {
        VineActions();
    }

    public void VineActions()
    {
        switch (vineType)
        {
            case VineType.Spike:
                animator.SetBool("isGrab", false);
                animator.SetBool("isHazard", false);
                if (gameObject.activeSelf)
                {
                    currentLifeTime -= Time.deltaTime;
                    if (currentLifeTime < 0)
                    {
                        currentLifeTime = lifeTime;
                        Destroy(gameObject);
                    }
                }
                break;

            case VineType.Grab:
                animator.SetBool("isGrab", true);
                break;

            case VineType.Hazard:
                animator.SetBool("isHazard", true);
                break;

            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collision logic: if the projectile collides with the player, raise the attack event and destroy the projectile.
        //If it collides with the ground, just destroy the projectile.
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHP.TakeDamage(damage, this);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Add player health change
            playerHP.TakeDamage(damage, this);
        }
    }
}