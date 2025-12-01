using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    #region Variables
    public TutorialPrompts abilityTutorial;
    [Space(20)]

    [Header("Video Player Setup")]
    public GameObject videoCanvas;
    public VideoClip[] videos; //Assign multiple clips if applicable
    public VideoPlayer videoPlayer;
    [Space(20)]

    [Header("Tutorial Text")]
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI abilityNameText;
    public string abilityName;
    public Button nextButton;

    private int activeLineIndex;
    private bool ifPickedUp = false;
    private AudioSource playerAttack;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        videoCanvas.SetActive(false);

        //Add listener for button clicks
        nextButton.onClick.AddListener(OnButtonClick);
        playerAttack = GameObject.Find("Character 1").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ifPickedUp)
        {
            TutorialMessage();
        }
    }

    public void WhenAbilityPickedUp(Component sender, object data)
    {
        if (sender is ItemPickup && sender.gameObject == this.gameObject)
        {
            videoCanvas.SetActive(true);
            ifPickedUp = true;
            GameManager.instance.StateSwitch(GameStates.Pause);
            playerAttack.enabled = false;
            PlayVideo(activeLineIndex);
        }
    }

    public void TutorialMessage()
    {
        tutorialText.text = abilityTutorial.textLines[activeLineIndex].text;

        #region Input Checks
        //Check which inputs are being pressed 
        bool keyInput = Keyboard.current?.eKey.wasPressedThisFrame ?? false;
        bool buttonInput = Gamepad.current?.buttonWest.wasPressedThisFrame ?? false;
        bool inputs = keyInput || buttonInput;
        
        #endregion

        if (inputs)
        {
            Confirm();
        }
    }

    #region Tutorial Input Check
    private void OnButtonClick()
    {
        //If this function is called
        Confirm();
    }

    private void Confirm()
    {
        activeLineIndex++;

        if(activeLineIndex >= abilityTutorial.textLines.Length)
        {
            activeLineIndex = 0;
            videoCanvas.SetActive(false);
            GameManager.instance.StateSwitch(GameStates.Play);
            playerAttack.enabled = true;
            Destroy(gameObject, 0.5f);
            return;
        }

        //Play next video
        PlayVideo(activeLineIndex);
    }
    #endregion

    #region Play Video Checks

    private void PlayVideo(int index)
    {
        if(videos != null && 
            index < videos.Length && videos[index] != null)
        {
            videoPlayer.clip = videos[index];   
            videoPlayer.Play();
        }
    }
    #endregion
}
