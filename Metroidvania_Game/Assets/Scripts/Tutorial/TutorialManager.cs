using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI tutorialText;
    public GameObject textBox;
    public Dialogue notifications;
    public int currentNotificationIndex = 0;
    [SerializeField]private float introTime = 0f;
    private bool tutorialStarted = false;

    public SceneInfo currentScene;
    public GameObject input;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input.SetActive(false);
        Introduction();
    }

    private void Update()
    {
        if(tutorialStarted)
        {
            introTime -= Time.deltaTime;
            if (introTime <= 0)
            {
                NextTutorialNotification();
            }
        }       
    }

    public void Introduction()
    {
        // Set the initial tutorial text to the first line of the notifications dialogue
        tutorialText.text = notifications.textLines[currentNotificationIndex].text;
        if(!tutorialStarted)
        {
            tutorialStarted = true;
        }
    }

    public void ShowTutorialText(bool show)
    {
        textBox.SetActive(show);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextTutorialNotification()
    {
        tutorialStarted = false;

        currentNotificationIndex++;
        // Check if the current notification index is within the bounds of the notifications text lines
        if (currentNotificationIndex < notifications.textLines.Length)
        {
            tutorialText.text = notifications.textLines[currentNotificationIndex].text;
            Debug.Log("Tutorial Notification: " + notifications.textLines[currentNotificationIndex].text);
        }
        else
        {
            SendBackToLevel();
        }
    }

    public void SendBackToLevel()
    {
        //Ensure that time scale is reset to normal before loading the next scene
        Time.timeScale = 1;
        SceneManager.LoadScene("Level 1");
    }
}
