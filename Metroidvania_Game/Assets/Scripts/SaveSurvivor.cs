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

    private SpriteRenderer npcSP;
    public TextMeshProUGUI survivorName;
    public TextMeshProUGUI dialog;

    private int activeLineIndex = 0;
    private bool conversationActive = false;
    private bool hasSurvivorBeenSaved = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcSP = GetComponent<SpriteRenderer>();
        npcSP.sprite = survivor.npcSprite;
        textBubble.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!conversationActive || conversation.textLines.Length == 0) { return; }

       

        dialog.text = conversation.textLines[activeLineIndex].text;
        survivorName.text = survivor.npcName;
        if (Keyboard.current.eKey.wasPressedThisFrame && playerIsNear)
        { 
            textBubble.SetActive(true);
            AdvanceDialog();
            if (hasSurvivorBeenSaved) { Destroy(gameObject); }
        }
    }

    void AdvanceDialog()
    {
        activeLineIndex += 1;

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
