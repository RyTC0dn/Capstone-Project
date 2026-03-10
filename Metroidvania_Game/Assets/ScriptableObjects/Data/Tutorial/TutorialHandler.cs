using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public enum TutorialCondition
{
    PressConfirm,
    OpenMenu,
    ClickButton,
    NextPage,
    None
}

public enum TutorialType
{
    UI,
    Movement,
    NPC,
    Combat,
    Dash,
    Interact
}

public class TutorialHandler : MonoBehaviour
{
    public TutorialSequence sequence;
    public TutorialType type;
    public SceneInfo sceneInfo;

    public TextMeshProUGUI tutorialText;
    public GameObject textBox;
    public GameObject[] arrows;

    [SerializeField] private int stepIndex = 0;
    private bool controlDetected;

    public TextMeshProUGUI currentStarCount;
    public TextMeshProUGUI maxStarCount;
    public GameObject keyInput;
    public GameObject buttonInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        controlDetected = sceneInfo.OnDeviceChange(Gamepad.current);
        StartTutorial();
    }

    // Update is called once per frame
    private void Update()
    {
        Tutorials();
    }

    private void StartTutorial()
    {
        TutorialStep step = sequence.steps[stepIndex];
        if (type == TutorialType.UI)
        {
            arrows[step.arrowIndex].SetActive(true);
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;

        currentStarCount.text = stepIndex.ToString();
        maxStarCount.text = sequence.steps.Length.ToString();

        textBox.SetActive(true);

        ShowStep();
    }

    private void ShowStep()
    {
        TutorialStep step = sequence.steps[stepIndex];

        tutorialText.text = step.dialogueText;

        currentStarCount.text = stepIndex.ToString();
        maxStarCount.text = sequence.steps.Length.ToString();

        if (type == TutorialType.UI)
        {
            foreach (var arrow in arrows)
                arrow.SetActive(false);

            if (step.arrowIndex >= 0 && step.arrowIndex < arrows.Length)
                arrows[step.arrowIndex].SetActive(true);
        }
        else
        {
            arrows[stepIndex].SetActive(false);
        }
    }

    private void NextStep()
    {
        stepIndex++;
        if (stepIndex >= sequence.steps.Length)
        {
            FinishTutorial();
            return;
        }

        ShowStep();
    }

    public void Tutorials()
    {
        keyInput.SetActive(sceneInfo.OnDeviceChange(Keyboard.current));
        buttonInput.SetActive(sceneInfo.OnDeviceChange(Gamepad.current));

        switch (type)
        {
            case TutorialType.UI:
                CheckProgression();
                break;

            case TutorialType.Movement:
                break;

            case TutorialType.NPC:
                break;

            case TutorialType.Combat:
                break;

            case TutorialType.Dash:
                break;

            case TutorialType.Interact:
                InteractProgression();
                break;

            default:
                break;
        }
    }

    #region UI Sequence

    private void CheckProgression()
    {
        TutorialStep step = sequence.steps[stepIndex];

        switch (step.condition)
        {
            case TutorialCondition.PressConfirm:
                if (!controlDetected)
                {
                    keyInput.SetActive(true);
                    buttonInput.SetActive(false);
                }
                else if (controlDetected)
                {
                    buttonInput.SetActive(true);
                    keyInput.SetActive(false);
                }
                if (Keyboard.current.eKey.wasPressedThisFrame
                    || Gamepad.current?.buttonWest.wasPressedThisFrame == true)
                    NextStep();
                break;

            case TutorialCondition.OpenMenu:
                TutorialManager.Instance.bookAnim.Play("BookEnter");
                if (Keyboard.current.tabKey.wasPressedThisFrame
                    || Gamepad.current?.selectButton.wasPressedThisFrame == true)
                    NextStep();
                break;

            case TutorialCondition.ClickButton:
                TutorialManager.Instance.bookAnim.Play("BookLeave");
                if (UIEvents.buttonClicked)
                    NextStep();
                break;

            case TutorialCondition.NextPage:
                if (UIEvents.pageTurned)
                    NextStep();
                break;

            case TutorialCondition.None:
                break;

            default:
                break;
        }
    }

    public void OnButtonClick(bool click)
    {
        UIEvents.buttonClicked = click;
    }

    public void OnPageTurn(bool turn)
    {
        UIEvents.pageTurned = turn;
    }

    #endregion UI Sequence

    private void InteractProgression()
    {
        TutorialStep step = sequence.steps[stepIndex];

        switch (step.condition)
        {
            case TutorialCondition.PressConfirm:
                if (!controlDetected)
                {
                    keyInput.SetActive(true);
                    buttonInput.SetActive(false);
                }
                else if (controlDetected)
                {
                    buttonInput.SetActive(true);
                    keyInput.SetActive(false);
                }
                if (Keyboard.current.eKey.wasPressedThisFrame
                    || Gamepad.current?.buttonWest.wasPressedThisFrame == true)
                    NextStep();
                break;

            case TutorialCondition.OpenMenu:
                TutorialManager.Instance.bookAnim.Play("BookEnter");
                if (Keyboard.current.tabKey.wasPressedThisFrame
                    || Gamepad.current?.selectButton.wasPressedThisFrame == true)
                    NextStep();
                break;

            case TutorialCondition.ClickButton:
                TutorialManager.Instance.bookAnim.Play("BookLeave");
                if (UIEvents.buttonClicked)
                    NextStep();
                break;

            case TutorialCondition.NextPage:
                if (UIEvents.pageTurned)
                    NextStep();
                break;

            case TutorialCondition.None:
                break;

            default:
                break;
        }
    }

    private void FinishTutorial()
    {
        if (type == TutorialType.UI)
            Time.timeScale = 1;
        textBox.SetActive(false);
        foreach (var arrow in arrows)
        {
            arrow.gameObject.SetActive(false);
        }

        TutorialManager.Instance.SendBackToLevel();
    }
}

//Tutorial Events
public static class UIEvents
{
    public static bool buttonClicked;
    public static bool pageTurned;
}