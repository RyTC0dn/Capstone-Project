using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class SaveStation : MonoBehaviour
{
    [SerializeField] private Image buttonPrompt;
    [SerializeField] private TextMeshProUGUI buttonText;

    [SerializeField] private Animator saveAnimator;

    private bool isDetected;
    private float animTime;

    private void Awake()
    {
        isDetected = false;
        buttonPrompt.enabled = false;
        buttonText.enabled = false;
        saveAnimator.SetBool("isSaving", false);
        animTime = 1;
    }

    private void FixedUpdate()
    {
        bool keyInput = Keyboard.current?.eKey.isPressed ?? false;
        bool buttonInput = Gamepad.current?.xButton.isPressed ?? false;

        if (keyInput || buttonInput && isDetected)
        {
            saveAnimator.SetBool("isSaving", true);
            animTime -= Time.deltaTime;
        }
        else if (animTime <= 0)
        {
            saveAnimator.SetBool("isSaving", false);
            animTime = 1;
        }



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            buttonPrompt.enabled = true;
            isDetected = true;
            buttonText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            buttonPrompt.enabled = false;
            isDetected = false;
            buttonText.enabled = false;
        }
    }

}
