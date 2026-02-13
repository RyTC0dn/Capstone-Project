using System.Collections;
using TMPro;
using UnityEngine;

public class ScreenShakeTimer : MonoBehaviour
{
    [Header("Camera Shake")]
    public Transform camTransform;

    public float shakeDuration = 0f;

    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector2 originalPos;

    [Header("Timer")]
    private float totalTime = 10f;
    public float currentTime;
    public TextMeshProUGUI timerText;

    private void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }

        currentTime = totalTime;
    }

    private void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    private void Update()
    {
        if(shakeDuration > 0f)
        {
            camTransform.localPosition = originalPos + Random.insideUnitCircle * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = currentTime.ToString();
        }
    }
}
