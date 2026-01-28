using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// This script may need revision
/// </summary>

public class SaveSurvivor : MonoBehaviour
{
    #region Variables
    [SerializeField] private bool playerIsNear = false;
    private bool hasBeenSaved = false;
    private bool blacksmithFreed = false;

    [Header("NPC State Data")]
    public NPC npcData;
    public Dialogue beforeSavingDialogue;
    public Dialogue afterSavingDialogue;
    public GameObject textBubble;
    public SpriteRenderer bubbleSp;

    [Space(5)]

    [Header("Dialogue Text")]
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI dialogueText;

    private int activeLineIndex = 0;
    private bool conversationActive = false;
    private bool firstLineShown = false;
    private int numberOfHits = 3;
    private Animator animator;

    [Space(20)]

    [Header("Trapped state values")]
    public float floatYAmplitude = 0.5f;
    public float floatYSpeed = 2f;
    public float floatXAmplitude = 5.0f;
    public float floatXSpeed = 0.5f;
    [Space(5)]

    [Header("Saved state values")]
    [Tooltip("Manually assign where the npc will fall to")]
    [SerializeField] private Vector2 targetPos;
    [Tooltip("Manually set how fast the npc falls")]
    [SerializeField] private float fallingSpeed;
    [Space(5)]

    private Vector2 startPos;

    private Rigidbody2D rb2D;

    public string sceneName;

    public GameObject buttonPrompt;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        //Check if this NPC should be destroyed based on save data and current scene
        if(GameManager.instance.isBlacksmithSaved )
        {
            Destroy(gameObject);
            return;
        }

        textBubble.SetActive(true);
        buttonPrompt.SetActive(false);
        dialogueText.text = beforeSavingDialogue.textLines[activeLineIndex].text;
        animator.enabled = false;
        npcName.text = npcData.npcName;

        Color start = new Color(0, 255, 242, 0.5f);
        bubbleSp.color = start;
    }

    void FloatingIdle()
    {
        float yOffset = Mathf.Sin(Time.time * floatYSpeed) * floatYAmplitude;
        float xOffset = Mathf.Sin(Time.time * floatXSpeed) * floatXAmplitude;
        transform.position = startPos + new Vector2(xOffset, yOffset);
    }

    private void Saved()
    {
        buttonPrompt.SetActive(true);
        conversationActive = true;
        rb2D.position = Vector2.MoveTowards(rb2D.position, targetPos, fallingSpeed * Time.deltaTime);
        BeforeSavedDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBeenSaved)
        {
            Saved();
        }
        else if(!hasBeenSaved)
        {
            FloatingIdle();
            rb2D.gravityScale = 0;
        }

    }

    private void BeforeSavedDialogue()
    {
        Dialogue currentDialogue = afterSavingDialogue;

        if (!conversationActive || currentDialogue.textLines.Length == 0) { return; }        

        dialogueText.text = currentDialogue.textLines[activeLineIndex].text;        

        //Calling inputs in booleans require ? after current
        //and ?? as to say it is currently not pressed 
        bool keyInput = Keyboard.current?.eKey.wasPressedThisFrame ?? false;
        bool buttonInput = Gamepad.current?.buttonWest.wasPressedThisFrame ?? false;

        if ((keyInput||buttonInput) && playerIsNear)
        {
            if (!firstLineShown)
            {
                textBubble.SetActive(true);                
                firstLineShown = true;
                return; //Don't advance on first first press
            }
            buttonPrompt.SetActive(false);
            AdvanceDialog();
        }
    }

    void AdvanceDialog()
    {
        activeLineIndex++;

        //If the active index variable stays less than
        //the amount of text lines generated
        Dialogue currentDialogue = hasBeenSaved ? afterSavingDialogue : beforeSavingDialogue;
        if (activeLineIndex >= currentDialogue.textLines.Length)
        {
            activeLineIndex = 0;
            conversationActive = false;
            textBubble.SetActive(false);
            firstLineShown = false;
            GameManager.instance.isBlacksmithSaved = true;
            PlayerPrefs.SetInt("BlacksmithSaved", 1);
            PlayerPrefs.Save();
            Destroy(gameObject);
            return;
        }

        //dialog.text = conversation.textLines[activeLineIndex].text;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            playerIsNear = true;
        }
        if (collision.CompareTag("Weapon"))
        {
            //Each time that the player hits
            numberOfHits--;
            StartCoroutine(BubbleHit(numberOfHits));
        }
    }

    IEnumerator BubbleHit(int hitsLeft)
    {
        yield return new WaitForSeconds(0.5f);

        if (hitsLeft == 2)
        {
            Color firstHit = new Color(255, 215, 0, 0.5f);
            bubbleSp.color = firstHit;
        }
        else if (hitsLeft == 1)
        {
            Color secondHit = new Color(255, 25, 0, 0.5f);
            bubbleSp.color = secondHit;
        }
        else if (hitsLeft <= 0)
        {
            bubbleSp.gameObject.SetActive(false);
            textBubble.gameObject.SetActive(false);
            numberOfHits = 3;
            animator.enabled = true;
            hasBeenSaved = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = false;
            buttonPrompt.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPos, 1f);
    }


}

