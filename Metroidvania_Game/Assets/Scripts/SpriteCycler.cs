using UnityEngine;
using UnityEngine.UI;

public class SpriteCycler : MonoBehaviour
{
    public Image sp;
    private int spriteIndex = 0;
    public Sprite[] sprites;
    public bool loop = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sp = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!loop)
            return;

        spriteIndex = (spriteIndex + 1) % sprites.Length;
        sp.sprite = sprites[spriteIndex];
    }
}