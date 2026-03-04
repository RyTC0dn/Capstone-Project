using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// Provides functionality for displaying tutorial previews, assigning instructional text, and transitioning to tutorial
/// scenes within a Unity application.
/// </summary>
/// <remarks>This component manages the presentation of tutorial content, including video previews and text
/// prompts, and handles scene transitions for tutorials. It is intended to be attached to a Unity GameObject and relies
/// on assigned references for video clips, UI elements, and dialogue prompts. Ensure that all required fields are
/// properly assigned in the Unity Editor before use.</remarks>
public class Tutorial : MonoBehaviour
{
    [Header("Screen Setup")]
    [SerializeField]private GameObject screen;
    public Dialogue textPrompts;
    [Tooltip("Assign in order of tutorial scenes")]
    public GameObject[] buttons;
    [SerializeField]private VideoPlayer player;
    [SerializeField] private TextMeshProUGUI infoText;
    public VideoClip[] clips; //manually assign clips

    private void Start()
    {
        foreach (var button in buttons)
        {
           button.SetActive(false);
        }
        infoText.enabled = false;
        screen.SetActive(false);
    }

    public void ShowPreview(int clipIndex)
    {
        screen.SetActive(true);
        if (clips.Length == 0)
        {
            Debug.LogWarning("No video clips assigned to video storage");
            return;
        }
        player.clip = clips[clipIndex];
        player.Play();
    }

    public void TextAssign(int textIndex)
    {
        infoText.enabled = true;
        infoText.text = textPrompts.textLines[textIndex].text;
        Show(textIndex);
    }

    public void TutorialSend(string tutorialScene)
    {
        //Ensure time scale is reset to normal before loading the tutorial scene
        Time.timeScale = 1f;
        MenuManager.instance.menuOpened = false;

        MenuManager.instance.CloseMenus();

        SceneManager.LoadScene(tutorialScene);
    }

    void Show(int index)
    {
        Close(); //Close all buttons before activating the assigned index
        buttons[index].SetActive(true);
    }

    void Close()
    {
        foreach(var button in buttons)
        {
            button.SetActive(false);
        }
    }
}
