using UnityEngine;

public class VineLogic : MonoBehaviour
{
    private int damage = 1;
    public Animator animator;
    private PlayerHealth playerHP;

    [SerializeField]
    private float lifeTime = 2.0f;

    private float currentLifeTime;

    public bool isSpike, isHazard, isGrab = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerHP = FindFirstObjectByType<PlayerHealth>();
        currentLifeTime = lifeTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameObject.activeSelf)
        {
            currentLifeTime -= Time.deltaTime;
            if (currentLifeTime < 0)
            {
                currentLifeTime = lifeTime;
                Destroy(gameObject);
            }
        }
    }

    public void VineActions(bool spike, bool grab, bool hazard)
    {
        if (spike)
        {
            animator.Play("Vine_Spike");
        }
        else if (grab)
        {
            animator.Play("Vine_Grab");
        }
        else if (hazard)
        {
            animator.Play("Vine_Hazard");
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