using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// This script will handle what goes on within the individual tutorial scene
/// </summary>
/// 
[RequireComponent(typeof(BoxCollider2D))]
public class TutorialScene : MonoBehaviour
{
    public GameObject tutorialSpawn;
    public GameObject textBackDrop;
    [SerializeField] private int messageIndex;
    public Dialogue tutorialMessages;
    public TextMeshProUGUI messageText;
    bool tutorialStart = false;


    private void Awake()
    {
        messageText.text = tutorialMessages.textLines[messageIndex].text;
        textBackDrop.SetActive(false);
    }

    private void Update()
    {
        //EnablePlayer();        
    }

    public void Messages(Component sender, object data)
    {
        if(data is bool)
        {
            if((bool)data == true)
            {
                messageIndex++;
                messageText.text = tutorialMessages.textLines[messageIndex].text;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tutorialStart = true;
            textBackDrop.SetActive(true);
            messageText.text = tutorialMessages.textLines[messageIndex].text;
        }
    }
}
