using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public enum TutorialCondition
{
    PressConfirm,
    OpenMenu,
    ClickButton,
    NextPage,
    InventoryPage,
    QuestPage,
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
    [Header("Tutorial Setup")]
    public TutorialSequence sequence;

    public TutorialTrigger trigger;
    public TutorialType type;
    public SceneInfo sceneInfo;
    private bool eventTrigger = false;

    [Header("UI Setup")]
    public TextMeshProUGUI tutorialText;

    public GameObject textBox;
    public GameObject[] arrows;
    public GameObject tutorialMenuFirst;

    public int stepIndex = 0;
    private bool controlDetected;

    public TextMeshProUGUI currentStarCount;
    public GameObject starUI;
    public GameObject keyInput;
    public GameObject buttonInput;

    private bool confirmPressed;
    private bool confirmMove;
    private bool tutorialMenuOpened = false;
    private float inputBuffer = 0f;
    private float inputBufferTime = 0.15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartTutorial();
    }

    // Update is called once per frame
    private void Update()
    {
        //Boolean checks
        controlDetected = sceneInfo.OnDeviceChange(Gamepad.current);

        confirmPressed = (Keyboard.current?.eKey.wasPressedThisFrame ?? false) ||
            (Gamepad.current?.buttonWest.wasPressedThisFrame ?? false);

        confirmMove = ((Keyboard.current?.aKey.wasPressedThisFrame ?? false) || (Keyboard.current?.dKey.wasPressedThisFrame ?? false))
            || ((Gamepad.current?.leftStick.left.wasPressedThisFrame ?? false) || (Gamepad.current?.leftStick.right.wasPressedThisFrame ?? false));

        if (confirmPressed)
            inputBuffer = inputBufferTime;

        if (inputBuffer > 0)
        {
            inputBufferTime -= Time.unscaledDeltaTime;
        }

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

        currentStarCount.text = sequence.steps[stepIndex].ToString();

        TutorialManager.Instance.bookAnim.gameObject.SetActive(false);
        textBox.SetActive(true);

        ShowStep();
    }

    private void ShowStep()
    {
        TutorialStep step = sequence.steps[stepIndex];

        tutorialText.text = step.dialogueText;

        currentStarCount.text = stepIndex.ToString();

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

    public void NextStep()
    {
        stepIndex++;
        if (stepIndex >= sequence.steps.Length)
        {
            //FinishTutorial();
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
                MovementProgression();
                break;

            case TutorialType.NPC:
                NPCProgression();
                break;

            case TutorialType.Combat:
                CombatProgression();
                break;

            case TutorialType.Dash:
                DashSequence();
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

                if (inputBuffer > 0)
                {
                    TutorialManager.Instance.bookAnim.gameObject.SetActive(true);
                    TutorialManager.Instance.bookAnim.Play("BookEnter");
                    NextStep();
                    inputBufferTime = 0;
                }

                break;

            case TutorialCondition.OpenMenu:
                if (Keyboard.current.tabKey.wasPressedThisFrame
                    || Gamepad.current?.selectButton.wasPressedThisFrame == true)
                    NextStep();
                break;

            case TutorialCondition.ClickButton:
                //MenuManager.instance.EquipmentOpen();
                TutorialManager.Instance.bookAnim.Play("BookLeave");
                if (UIEvents.buttonClicked)
                    NextStep();
                break;

            case TutorialCondition.NextPage:
                if (UIEvents.pageTurned)
                    NextStep();
                break;

            case TutorialCondition.InventoryPage:
                if (UIEvents.nextPageTurn)
                    NextStep();
                break;

            case TutorialCondition.QuestPage:
                if (UIEvents.finalPageTurn)
                    NextStep();
                break;

            case TutorialCondition.None:
                MenuManager.instance.tutorialMenu.SetActive(true);
                starUI.SetActive(false);
                sceneInfo.bookIsLookedAt = true;
                MenuManager.instance.finalizeTutorialButton.interactable = true;
                foreach (var arrow in arrows)
                {
                    arrow.gameObject.SetActive(false);
                }
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
        Debug.Log("Turning page");
    }

    public void OnInventory(bool next)
    {
        UIEvents.nextPageTurn = next;
        Debug.Log("To next page");
    }

    public void OnQuest(bool final)
    {
        UIEvents.finalPageTurn = final;
        Debug.Log("To final page");
    }

    #endregion UI Sequence

    #region Interaction Sequence

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

                if (inputBuffer > 0)
                {
                    NextStep();
                    inputBufferTime = 0;
                }
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

    #endregion Interaction Sequence

    #region Movement Sequence

    private void MovementProgression()
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

                if (inputBuffer > 0)
                {
                    NextStep();
                    inputBufferTime = 0;
                }
                break;

            case TutorialCondition.OpenMenu:
                if (confirmMove)
                {
                    NextStep();
                }
                break;

            case TutorialCondition.ClickButton:
                if (Keyboard.current.spaceKey.wasPressedThisFrame
                    || Gamepad.current?.buttonSouth.wasPressedThisFrame == true)
                {
                    NextStep();
                }
                break;

            case TutorialCondition.NextPage:
                if (eventTrigger)
                {
                    NextStep();
                    eventTrigger = false;
                }
                break;

            case TutorialCondition.InventoryPage:

                if (!tutorialMenuOpened)
                {
                    MenuManager.instance.TutorialOpen(tutorialMenuFirst);
                    tutorialMenuOpened = true;
                }

                sceneInfo.isMoved = true;
                MenuManager.instance.finalizeTutorialButton.interactable = true;
                break;

            default:
                break;
        }
    }

    #endregion Movement Sequence

    #region DashSequence

    private void DashSequence()
    {
        TutorialStep step = sequence.steps[stepIndex];

        switch (step.condition)
        {
            case TutorialCondition.PressConfirm:
                if (Keyboard.current.rKey.wasPressedThisFrame
                    || Gamepad.current?.leftShoulder.wasPressedThisFrame == true)
                {
                    NextStep();
                }
                break;

            case TutorialCondition.OpenMenu:

                break;

            case TutorialCondition.ClickButton:
                break;

            case TutorialCondition.NextPage:
                if (!tutorialMenuOpened)
                {
                    MenuManager.instance.TutorialOpen(tutorialMenuFirst);
                    tutorialMenuOpened = true;
                }

                sceneInfo.dashed = true;
                MenuManager.instance.finalizeTutorialButton.interactable = true;
                foreach (var arrow in arrows)
                {
                    arrow.gameObject.SetActive(false);
                }
                break;

            case TutorialCondition.InventoryPage:

                break;

            default:
                break;
        }
    }

    #endregion DashSequence

    #region NPC Sequence

    private void NPCProgression()
    {
        TutorialStep step = sequence.steps[stepIndex];

        switch (step.condition)
        {
            case TutorialCondition.PressConfirm:
                if (Mouse.current.leftButton.wasPressedThisFrame
                    || Gamepad.current?.rightTrigger.wasPressedThisFrame == true)
                {
                    NextStep();
                }
                break;

            case TutorialCondition.OpenMenu:
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
                {
                    NextStep();
                }
                break;

            case TutorialCondition.ClickButton:

                break;

            case TutorialCondition.NextPage:
                if (NPCEvents.isDetected && (Keyboard.current.eKey.wasPressedThisFrame
                    || Gamepad.current?.buttonWest.wasPressedThisFrame == true))
                    NextStep();
                break;

            case TutorialCondition.InventoryPage:
                if (!tutorialMenuOpened)
                {
                    MenuManager.instance.TutorialOpen(tutorialMenuFirst);
                    tutorialMenuOpened = true;
                }
                sceneInfo.npcInteracted = true;
                MenuManager.instance.finalizeTutorialButton.interactable = true;
                foreach (var arrow in arrows)
                {
                    arrow.gameObject.SetActive(false);
                }
                break;

            default:
                break;
        }
    }

    public void OnNPCFinale(bool detected)
    {
        NPCEvents.isDetected = detected;
    }

    #endregion NPC Sequence

    #region Combat Sequence

    private void CombatProgression()
    {
        TutorialStep step = sequence.steps[stepIndex];

        int totalEnemies = trigger.triggers.Length;

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
                if (inputBuffer > 0)
                {
                    NextStep();
                    inputBufferTime = 0;

                    if (trigger != null && trigger.triggers.Length > 0)
                        trigger.triggers[0].SetActive(true);
                }

                break;

            case TutorialCondition.OpenMenu:

                break;

            case TutorialCondition.ClickButton:

                break;

            case TutorialCondition.NextPage:
                if (!tutorialMenuOpened)
                {
                    MenuManager.instance.TutorialOpen(tutorialMenuFirst);
                    tutorialMenuOpened = true;
                }
                sceneInfo.isMoved = true;
                MenuManager.instance.finalizeTutorialButton.interactable = true;
                foreach (var arrow in arrows)
                {
                    arrow.gameObject.SetActive(false);
                }
                break;

            case TutorialCondition.InventoryPage:

                break;

            default:
                break;
        }
    }

    #endregion Combat Sequence
}

//Tutorial Events
public static class UIEvents
{
    public static bool buttonClicked;
    public static bool pageTurned;
    public static bool nextPageTurn;
    public static bool finalPageTurn;
}

public static class NPCEvents
{
    public static bool isDetected;
    public static bool activated;
}