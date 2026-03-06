using System.Collections;
using UnityEngine;

/// <summary>
/// Provides functionality for applying and managing knockback effects on a GameObject in a 2D physics environment.
/// </summary>
/// <remarks>This component is intended to be attached to a GameObject with a Rigidbody2D. It enables the
/// GameObject to respond to knockback forces, such as those resulting from collisions or attacks, by applying
/// directional forces and managing the knockback state. The class exposes configurable parameters for knockback
/// duration and force magnitudes, and tracks whether the GameObject is currently being knocked back via the
/// IsBeingKnockedBack property.</remarks>
public class Knockback : MonoBehaviour
{
    public float knockbackTime = 0.2f;
    public float hitDirectionForce = 10f;
    public float constForce = 5;
    public float inputForce = 7.5f;

    public bool IsBeingKnockedBack { get; private set; }

    public Rigidbody2D rb2D;

    private Coroutine knockbackCoroutine;

    public IEnumerator KnockBackAction(Vector2 hitDirection, Vector2 constForceDirection, float inputDirection)
    {
        IsBeingKnockedBack = true;

        Vector2 _hitForce;
        Vector2 _constForce;
        Vector2 _knockbackForce;
        Vector2 _combinedForce;

        _hitForce = hitDirection * hitDirectionForce;
        _constForce = constForceDirection * constForce;

        float elapsedTime = 0f;
        while (elapsedTime < knockbackTime)
        {
            elapsedTime += Time.fixedDeltaTime;

            //Combine hit force and constant force
            _knockbackForce = _hitForce + _constForce;

            //Combine knockback force with input force
            if (inputDirection != 0)
            {
                _combinedForce = _knockbackForce + new Vector2(inputDirection, 0f);
            }
            else
            {
                _combinedForce = _knockbackForce;
            }

            rb2D.linearVelocity = _combinedForce;

            yield return new WaitForFixedUpdate();
        }

        IsBeingKnockedBack = false;
    }

    public void CallKnockback(Vector2 hitDirection, Vector2 constForceDirection, float inputDirection)
    {
        knockbackCoroutine = StartCoroutine(KnockBackAction(hitDirection, constForceDirection, inputDirection));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
}