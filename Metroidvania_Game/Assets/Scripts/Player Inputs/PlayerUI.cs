using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Text")]
    //Move inventory manager code here
    private bool menuOpened;

    [Header("HP Icon")]
    public List<Image> clockIcons = new List<Image>(); //Drag each UI Clock image in order
    public Sprite fullClockSprite;
    public Sprite brokenClockSprite;
    public List<Animator> clockAnimations = new List<Animator>(); //Drag Each clock animator in order  
    public GameObject damageFrame;

    [SerializeField]private int totalHealth = 4;
    AudioPlayer player;
    [SerializeField]private AudioSource source;


    private void Awake()
    {
        SetHealth(totalHealth);
        for (int i = 0; i < clockIcons.Count; i++)
        {
            var animator = clockAnimations[i].GetComponent<Animator>();
            animator.enabled = false;
        }
        damageFrame.SetActive(false);
        player = GetComponentInChildren<AudioPlayer>();
    }

    private void SetHealth(int health)
    {

        //Each clock will represent 2 HP
        int remainingHealth = health;

        for (int i = 0; i < clockIcons.Count; i++)
        {
            //This is to ensure to change the sprites whether the sprites
            //are using sprite renderers or UI image components
            var image = clockIcons[i] as Image;
            var spriteRenderer = clockIcons[i].GetComponent<SpriteRenderer>();
            var animator = clockAnimations[i].GetComponent<Animator>();

            if(remainingHealth >= 2)
            {
                //Sprite for unbroken clock at 2 HP
                if (image) image.sprite = fullClockSprite;
                if(spriteRenderer) spriteRenderer.sprite = fullClockSprite;
                if (image) image.enabled = true;
                if (spriteRenderer) spriteRenderer.enabled = true;

                //Initiate damage animation for player ui
                damageFrame.SetActive(true);
                StartCoroutine(DelayAnimation(damageFrame, 1.5f));

                remainingHealth -= 2;
            }
            else if (remainingHealth == 1)
            {
                //Sprite for broken clock at 1 HP
                if (image) image.sprite = brokenClockSprite;
                if (spriteRenderer) spriteRenderer.sprite = brokenClockSprite;
                if (image) image.enabled = true;
                if (spriteRenderer) spriteRenderer.enabled = true;

                damageFrame.SetActive(true);
                StartCoroutine(DelayAnimation(damageFrame, 1.5f));
                player.PlayRandomClip(source, 17, 21);

                remainingHealth -= 1;
            }
            else
            {
                //When no health left > hide icon
                animator.enabled = true;
                animator.Play("ClockBreak");
                if (player.clips != null)
                {
                    player.PlayRandomClip(source, 22, 25);
                }
                StartCoroutine(Delay(image, spriteRenderer, 1.5f));                
            }
        }
    }

    private IEnumerator DelayAnimation(GameObject frames, float delay)
    {
        yield return new WaitForSeconds(delay);
        frames.SetActive(false);
    }

    private IEnumerator Delay(Image image, SpriteRenderer spriteRenderer, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (image) image.enabled = false;
        if (spriteRenderer) spriteRenderer.enabled = false;        
    }

    public void UpdateHealth(Component sender, object data)
    {
        //If sender is PlayerHealth
        if(data is int)
        {
            int amount = (int)data;
            SetHealth(amount);
            Debug.Log($"Update health by {amount}");
        }        
    }
}
