using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TownNPCDialog : MonoBehaviour
{
    public NPC villager;
    public Dialogue conversation;
    public GameObject textBubble;

    [Header("Dialogue Settings")]
    public TextMeshProUGUI survivorName;
    public TextMeshProUGUI dialog;
    public AudioSource audioPlayer;

    private int activeLineIndex = 0;
    private bool conversationActive = false;
    private bool hasSurvivorBeenSaved = false;
    private bool firstLineShown = false;
    [SerializeField] private bool playerIsNear = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textBubble.SetActive(false);
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!conversationActive || conversation.textLines.Length == 0) { return; }



        dialog.text = conversation.textLines[activeLineIndex].text;
        survivorName.text = villager.npcName;
        if (Keyboard.current.eKey.wasPressedThisFrame && playerIsNear)
        {
            if (!firstLineShown)
            {
                textBubble.SetActive(true);
                firstLineShown = true;

                PlayAudio(activeLineIndex);
                return; //Don't advance on first first press
            }

            AdvanceDialog();
            if (hasSurvivorBeenSaved) { Destroy(gameObject); }
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
            textBubble.SetActive(false);
            hasSurvivorBeenSaved = true;
            return;
        }

        PlayAudio(activeLineIndex);
    }

    void PlayAudio(int index)
    {
        if(conversation.audioVoice != null && index < conversation.textLines.Length)
        {
            audioPlayer.clip = conversation.audioVoice[index];
            audioPlayer.Play();
        }
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
