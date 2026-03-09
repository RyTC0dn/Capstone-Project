using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    public Character character;

    [Header("UI Text")]
    private bool menuOpened;

    [Header("HP Icon")]
    public List<Image> clockIcons = new List<Image>(); //Drag each UI Clock image in order

    public Sprite fullClockSprite;
    public Sprite brokenClockSprite;
    public List<Animator> clockAnimations = new List<Animator>(); //Drag Each clock animator in order
    public GameObject damageFrame;

    [SerializeField] private int totalHealth = 4;
    private AudioPlayer player;
    [SerializeField] private AudioSource source;

    [Space(20)]
    [Header("Reminder Setup")]
    public Dialogue reminder;

    [SerializeField] private int currentDialogue = 0;
    private float timer = 2.5f;

    public GameObject textBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;
    public TypewriterEffect typewriter;

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
        typewriter = player.GetComponent<TypewriterEffect>();

        switch (character)
        {
            case Character.Knight:
                nameText.text = "Knight";
                break;

            case Character.Cleric:
                nameText.text = "Cleric";
                break;

            case Character.Huntress:
                nameText.text = "Huntress";
                break;

            case Character.Wizard:
                nameText.text = "Wizard";
                break;

            default:
                break;
        }

        reminder.npcName = nameText.text;
        textBox.SetActive(false);
    }

    private void Update()
    {
        HandleReminders();
    }

    /// <summary>
    /// Handles the display and progression of reminder messages based on the player's current dialogue state and saved
    /// game progress.
    /// </summary>
    /// <remarks>This method determines which reminder to show depending on which characters have been saved
    /// in the game and the current dialogue index. It also advances the dialogue when the player provides input and
    /// hides the reminder text box when appropriate. This method should be called during the dialogue flow to ensure
    /// reminders are shown and dismissed correctly.</remarks>
    private void HandleReminders()
    {
        if (reminder == null || dialogueText == null || textBox == null) return;

        //Check if the player presses input
        bool key = Keyboard.current?.eKey.isPressed ?? false;
        bool button = Gamepad.current?.buttonWest.isPressed ?? false;
        bool input = key || button;

        //Determine which reminder should show based on Game Manager save instance
        if (GameManager.instance.isBlackSmithSaved && currentDialogue == 0 && !textBox.activeSelf)
        {
            ShowReminders(reminder.textLines[0].text);
            return;
        }
        else if (GameManager.instance.isPotionMakerSaved && currentDialogue == 1 && !textBox.activeSelf)
        {
            ShowReminders(reminder.textLines[1].text);
            return;
        }
        else if (GameManager.instance.isHealerSaved && currentDialogue == 2 && !textBox.activeSelf)
        {
            ShowReminders(reminder.textLines[2].text);
            return;
        }

        if (textBox.activeSelf)
        {
            bool isTyping = typewriter != null ? typewriter.IsTyping : false;

            timer -= Time.deltaTime;

            if (!isTyping && input && timer <= 0)
            {
                currentDialogue++;
                textBox.SetActive(false);
            }
        }
    }

    private void ShowReminders(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        textBox.SetActive(true);

        if (typewriter != null)
        {
            typewriter.SetText(text, dialogueText);
        }
        else
        {
            dialogueText.text = text;
        }
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

            if (remainingHealth >= 2)
            {
                //Sprite for unbroken clock at 2 HP
                if (image) image.sprite = fullClockSprite;
                if (spriteRenderer) spriteRenderer.sprite = fullClockSprite;
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
        if (data is int)
        {
            int amount = (int)data;
            SetHealth(amount);
            Debug.Log($"Update health by {amount}");
        }
    }
}