using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIMenuTutorial : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public GameObject textBox;
    public GameObject keyboardIcon;
    public GameObject controllerIcon;
    public GameObject[] arrows;
    [SerializeField] private Animator bookAnim;
    [Space(20)]

    public Dialogue notifications;
    [SerializeField]private int currentNotificationIndex = 0;
    private bool controllerDetected;

    public SceneInfo info;
    private float timeNext = 2f;
    private bool tutorialStart = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textBox.SetActive(false);
        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(false);
        }

        //Have the book ignore time scale
        bookAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        controllerDetected = info.OnDeviceChange(Gamepad.current);

        Tutorial();
    }

    // Update is called once per frame
    void Update()
    {        
        bool input = controllerDetected ? Gamepad.current.buttonWest.wasPressedThisFrame : Keyboard.current.eKey.wasPressedThisFrame;

        #region Detect Controller or Keyboard Input
        if (controllerDetected && tutorialStart)
        {
            controllerIcon.SetActive(true);
            keyboardIcon.SetActive(false);
        }
        else if (!controllerDetected && tutorialStart)
        {
            controllerIcon.SetActive(false);
            keyboardIcon.SetActive(true);
        }
        else
        {
            controllerIcon.SetActive(false);
            keyboardIcon.SetActive(false);
        }
        #endregion

        #region Tutorial Sequence Progression
        if (tutorialStart && input)
        {
            NextTutorialNotification(true);
            tutorialStart = false; // Stop the tutorial from automatically progressing after the first notification
        }
        else if (!tutorialStart && currentNotificationIndex == 1)
        {
            OpenMenu();
        }
        else if (currentNotificationIndex == 2)
        {
            tutorialStart = true; // Start the tutorial progression for the next notifications
        }
        else if(currentNotificationIndex == 5 && input)
        {
            FinishTutorial();
            //Turn off all arrows
            foreach (GameObject arrow in arrows)
            {
                arrow.SetActive(false);
            }
        }
        #endregion
    }

    private void OpenMenu()
    {

        bool input = controllerDetected ? Gamepad.current.selectButton.wasPressedThisFrame : Keyboard.current.tabKey.wasPressedThisFrame;

        if (input)
        {
            //Have  icons show up in menu for this tutorial
            info.isAxeBought = true; // Set the axe as bought to trigger the next tutorial notification
            info.isShieldPickedUp = true; // Set the shield as picked up to trigger the next tutorial notification
            info.isWallBreakPickedUp = true; // Set the wall break as picked up to trigger the next tutorial notification
            bookAnim.Play("BookLeave"); // Play the book exit animation
            arrows[0].SetActive(false); // Hide the first arrow
            NextTutorialNotification(true);
        }
    }

    // This method will be called to progress through the tutorial notifications. It takes a boolean parameter "activate"
    // which determines whether to move to the next notification or not.
    //This is to prevent unnecessary calls to this method when the player is not ready to progress through the tutorial
    public void NextTutorialNotification(bool activate)
    {
        // If activate is true, move to the next notification and show the corresponding arrow. If false, do nothing.
        if (activate && !info.bookIsLookedAt)
        {
            currentNotificationIndex++;
            if (currentNotificationIndex < notifications.textLines.Length)
            {
                tutorialText.text = notifications.textLines[currentNotificationIndex].text;
                arrows[currentNotificationIndex - 1].SetActive(true); // Show the corresponding arrow for the current notification
                Debug.Log("Tutorial Notification: " + notifications.textLines[currentNotificationIndex].text);

            }
        }
        else
        {
            return;
        }
    }

    //This method will be triggered at the start of the game in town scene, 
    // pausing the game and showing the first tutorial notification.
    public void Tutorial()
    {
        Time.timeScale = 0f; // Pause the game

        textBox.SetActive(true); // Show the tutorial text box
        tutorialStart = true;
        tutorialText.text = notifications.textLines[currentNotificationIndex].text; // Set the tutorial text to the first notification
        bookAnim.Play("BookEnter"); // Play the book animation
    }

    private void FinishTutorial()
    {
        Time.timeScale = 1f; // Resume the game
        info.bookIsLookedAt = true; // Set the book as looked at to trigger the next tutorial notification
        textBox.SetActive(false); // Hide the tutorial text box
        info.ResetSceneInfo(); // Reset the scene info for the next time the player goes through the tutorial
        Debug.Log("Finished tutorial");
    }

    private IEnumerator ColorArrows()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        if(arrows != null)
        {
            float elapsed = 0f;
            foreach (GameObject arrow in arrows)
            {
                Color color = arrow.GetComponent<Image>().color;
                while (elapsed < 1f)
                {
                    elapsed += Time.unscaledDeltaTime;
                    float alpha = Mathf.PingPong(Time.time * 4f, 0.5f * 5) + 0.5f / 2; // oscillates between 0.5–1
                    color.a = alpha;
                    arrow.GetComponent<Image>().color = color;
                    yield return null;
                }
                arrow.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1f); // Ensure it ends fully visible
            }
        }
    }
}
