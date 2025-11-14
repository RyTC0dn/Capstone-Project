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
    [SerializeField]private bool playerIsNear = false;

    public NPC npcData;
    public Dialogue activeDialogue;
    public GameObject textBubble;

    public TextMeshProUGUI npcName;
    public TextMeshProUGUI dialogueText;

    private int activeLineIndex = 0;
    private bool conversationActive = false;
    private bool firstLineShown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textBubble.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!conversationActive || activeDialogue.textLines.Length == 0) { return; }

       

        dialogueText.text = activeDialogue.textLines[activeLineIndex].text;
        npcName.text = npcData.npcName;

        if (Keyboard.current.eKey.wasPressedThisFrame && playerIsNear)
        { 
            if(!firstLineShown)
            {
                textBubble.SetActive(true);
                firstLineShown = true;
                return; //Don't advance on first first press
            }
            
            AdvanceDialog();
        }
    }

    void AdvanceDialog()
    {
        activeLineIndex++;

        //If the active index variable stays less than
        //the amount of text lines generated
        if (activeLineIndex >= activeDialogue.textLines.Length)
        {
            activeLineIndex = 0;
            conversationActive = false;
            textBubble.SetActive(false );
            GameManager.instance.isNPCSaved = true;
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
            conversationActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
