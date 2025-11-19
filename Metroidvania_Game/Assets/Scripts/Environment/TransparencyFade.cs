using UnityEngine;

public class TransparencyFade : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float fadedAlpha = 0.4f;
    public float fadeSpeed = 5f;

    private float targetAlpha = 1f;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            targetAlpha = fadedAlpha;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            targetAlpha = 1f;
    }

    private void Update()
    {
        Color c = spriteRenderer.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
        spriteRenderer.color = c;
    }
}
