using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class WallBreaking : MonoBehaviour
{
    private ParticleSystem particle;
    private SpriteRenderer sr;
    private BoxCollider2D bc;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string weapon = "Weapon";
        string ability = "AbilityPickup";

        if (collision.gameObject.CompareTag(ability))
        {
            StartCoroutine(Break());
        }
    }

    private IEnumerator Break()
    {
        particle.Play();
        sr.enabled = false;
        bc.enabled = false;

        yield return new WaitForSeconds(particle.main.startLifetime.constantMax);

        Destroy(gameObject);
        Debug.Log("Particle System activated");
    }
}
