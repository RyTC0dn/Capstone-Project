using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI tutorialText;
    public Dialogue notifications;
    public int currentNotificationIndex = 0;
    [SerializeField]private float introTime = 0f;
    private bool tutorialStarted = false;

    public SceneInfo currentScene;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

        if (currentScene.isMoved)
        {
            SendBackToLevel();
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

    public void ResetTutorial()
    {
        currentNotificationIndex = 0;
        tutorialText.text = "";
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
        if (currentNotificationIndex < notifications.textLines.Length)
        {
            tutorialText.text = notifications.textLines[currentNotificationIndex].text;
            Debug.Log("Tutorial Notification: " + notifications.textLines[currentNotificationIndex].text);
        }
    }

    public void SendBackToLevel()
    {
        SceneManager.LoadScene("Level1");
    }
}
