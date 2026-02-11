using System.Collections;
using UnityEngine;

public class FallingBlocks : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1.0f;

    [SerializeField] private Rigidbody2D rb2D;


    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb2D.bodyType = RigidbodyType2D.Dynamic;

    }
}
