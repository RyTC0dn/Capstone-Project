using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI tutorialText;
    public GameObject textBox;
    public Dialogue notifications;
    public int currentNotificationIndex = 0;
    [SerializeField] private float introTime = 0f;
    private bool tutorialStarted = false;
    public string sceneName;

    [Space(20)]
    public TextMeshProUGUI currentStarCount;

    public TextMeshProUGUI maxStarCount;

    public SceneInfo sceneInfo;
    public GameObject input;
    public Animator bookAnim;
    public Button[] button;

    [Tooltip("Assign only in the combat tutorial scene")]
    public GameObject[] enemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentStarCount.text = currentNotificationIndex.ToString();
        maxStarCount.text = notifications.textLines.Length.ToString();

        input.SetActive(false);
        Introduction();

        //string sceneName = SceneManager.GetActiveScene().name;

        //Robust comparison: trim and ignore case to avoid issues with scene name formatting
        if (sceneName.Trim().Equals("Tutorial 5", System.StringComparison.OrdinalIgnoreCase))
        {
            if (sceneInfo != null)
            {
                sceneInfo.isWallBreakPickedUp = true;
                Debug.Log("Wallbreak tutorial trigger set to true");
            }
            else
            {
                Debug.LogWarning("SceneInfo reference is not assigned in TutorialTrigger for Wallbreak tutorial.");
            }
        }
        else if (sceneName.Trim().Equals("Tutorial 1", System.StringComparison.OrdinalIgnoreCase))
        {
            sceneInfo.isWallBreakPickedUp = true;
            sceneInfo.isAxeBought = true;
            sceneInfo.isShieldPickedUp = true;
            bookAnim.Play("BookEnter"); //On start, have the book enter animation trigger
        }
        else
        {
            sceneInfo.ResetSceneInfo();
        }
    }

    private void Update()
    {
        if (tutorialStarted)
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
        if (!tutorialStarted)
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
            currentStarCount.text = currentNotificationIndex.ToString(); //Update text
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