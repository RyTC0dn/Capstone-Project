using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Breaking the wall when the player uses the wall break dash ability 
/// </summary>
public class WallBreaking : MonoBehaviour
{
    private ParticleSystem particle;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private float popUpDistance;
    private GameObject player;

    private void Awake()
    {
        //Initializing variables
        particle = GetComponentInChildren<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        player = GameObject.Find("Character 1");

        //Setting activity of objects/components
        icon.enabled = false;
    }

    private void Update()
    {
        //Ability icon shows up if the player is within a manually set pop-up distance 
        if (Vector2.Distance(transform.position, 
            player.transform.position) < popUpDistance)
        {
            icon.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string ability = "AbilityPickup";

        //If the player has the ability tag
        if (collision.gameObject.CompareTag(ability))
        {
            StartCoroutine(Break()); //Call the break function
        }
    }

    private IEnumerator Break()
    {
        //Play the particle system then
        //disable the collider and sprite renderer
        particle.Play();
        sr.enabled = false;
        bc.enabled = false;

        yield return new WaitForSeconds(particle.main.startLifetime.constantMax);

        //Once the particle effect plays till the end of the total life time
        //destroy the game object
        Destroy(gameObject);
        Debug.Log("Particle System activated");
    }
}
