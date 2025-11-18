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
    private bool hasBeenSaved = false;

    public NPC npcData;
    public Dialogue beforeSavingDialogue;
    public Dialogue afterSavingDialogue;
    public GameObject textBubble;

    public TextMeshProUGUI npcName;
    public TextMeshProUGUI dialogueText;

    private int activeLineIndex = 0;
    private bool conversationActive = false;
    private bool firstLineShown = false;

    public string sceneName;

    public GameObject buttonPrompt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Check if this NPC should be destroyed based on save data and current scene
        if(PlayerPrefs.GetInt("BlacksmithSaved", 0) == 1 && 
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == sceneName)
        {
            Destroy(gameObject);
            return;
        }

        textBubble.SetActive(false);
        buttonPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        BeforeSavedDialogue();
    }

    private void BeforeSavedDialogue()
    {
        if (!conversationActive || beforeSavingDialogue.textLines.Length == 0) { return; }

        Dialogue currentDialogue = hasBeenSaved ? afterSavingDialogue : beforeSavingDialogue;

        dialogueText.text = currentDialogue.textLines[activeLineIndex].text;
        npcName.text = npcData.npcName;

        if (Keyboard.current.eKey.wasPressedThisFrame && playerIsNear)
        {
            if (!firstLineShown)
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
        if (activeLineIndex >= beforeSavingDialogue.textLines.Length)
        {
            activeLineIndex = 0;
            conversationActive = false;
            textBubble.SetActive(false );
            firstLineShown = false;
            //GameManager.instance.isBlacksmithSaved = true;
            //PlayerPrefs.SetInt("BlacksmithSaved", 1);
            //PlayerPrefs.Save();
            //Destroy(gameObject);
            return;
        }

        //dialog.text = conversation.textLines[activeLineIndex].text;
    }

    public void OnSave(Component sender, object data)
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {       

        if (collision.CompareTag("Player"))
        {
            playerIsNear = true;
            conversationActive = true;
            buttonPrompt.SetActive(true);
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
}
