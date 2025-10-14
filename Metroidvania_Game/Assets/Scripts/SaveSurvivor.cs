using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SaveSurvivor : MonoBehaviour
{
    [SerializeField]private bool playerIsNear = false;

    public NPC survivor;
    public Dialogue conversation;
    public GameObject textBubble;

    public TextMeshProUGUI survivorName;
    public TextMeshProUGUI dialog;
    public GameObject EndText;

    private int activeLineIndex = 0;
    private bool conversationActive = false;
    private bool hasSurvivorBeenSaved = false;
    private bool firstLineShown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textBubble.SetActive(false);
        EndText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!conversationActive || conversation.textLines.Length == 0) { return; }

       

        dialog.text = conversation.textLines[activeLineIndex].text;
        survivorName.text = survivor.npcName;
        if (Keyboard.current.eKey.wasPressedThisFrame && playerIsNear)
        { 
            if(!firstLineShown)
            {
                textBubble.SetActive(true);
                firstLineShown = true;
                return; //Don't advance on first first press
            }
            
            AdvanceDialog();
            if (hasSurvivorBeenSaved) { GameManager.instance.hasSavedBlacksmith = true; EndText.SetActive(true);
                Destroy(gameObject); }
        }
    }

    void AdvanceDialog()
    {
        activeLineIndex++;

        //If the active index variable stays less than
        //the amount of text l ines generated
        if (activeLineIndex >= conversation.textLines.Length)
        {
            activeLineIndex = 0;
            conversationActive = false;
            textBubble.SetActive(false );
            hasSurvivorBeenSaved = true;
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
}
