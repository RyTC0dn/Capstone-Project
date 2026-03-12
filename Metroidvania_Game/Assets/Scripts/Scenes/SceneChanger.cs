using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public string sceneDestination;
    [SerializeField] private string spawnPointDestination;
    [SerializeField] private float transportTimer;
    [SerializeField] private Image chargeBar;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Image buttonPrompt;
    private Canvas timeCanvas;
    private bool isDetected;

    private void Awake()
    {
        timeCanvas = GameObject.Find("TimerCanvas").GetComponent<Canvas>();
        chargeBar = GameObject.Find("TimerFill").GetComponent<Image>();

        timeCanvas.enabled = false;
        buttonPrompt.enabled = false;
        isDetected = false;
        buttonText.enabled = false;
    }

    private void FixedUpdate()
    {
        bool keyInput = Keyboard.current?.eKey.isPressed ?? false;
        bool buttonInput = Gamepad.current?.xButton.isPressed ?? false;
        if ((keyInput || buttonInput) && isDetected && gameObject.tag == "LevelEnter")
        {
            SceneManager.LoadScene(sceneDestination);
            GameManager.instance.nextSpawnPointName = spawnPointDestination;
            isDetected = false;
        }
        else if ((keyInput || buttonInput) && isDetected && gameObject.tag == "LevelExit")
        {
            SceneManager.LoadScene(sceneDestination);
            isDetected = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buttonPrompt.enabled = true;
            isDetected = true;
            buttonText.enabled = true;
            Debug.Log("Player is Detected");
            //StartCoroutine(TimeToChange(transportTimer));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //chargeBar.fillAmount = 0;
            //timeCanvas.enabled = false;
            buttonPrompt.enabled = false;
            isDetected = false;
            buttonText.enabled = false;
        }
    }

    ///Ryan's added functions
    /// <summary>
    /// Ryan's added functions
    /// </summary>
    /// If player has 0 hp
    public void OnPlayerDeath(Component sender, object data)
    {
        if (sender is PlayerHealth)
        {
            GameManager.instance.nextSpawnPointName = spawnPointDestination;
            SceneManager.LoadScene(sceneDestination);
        }
    }

    //private IEnumerator TimeToChange(float duration)
    //{
    //    timeCanvas.enabled = true;
    //    if (duration <= 0f)
    //    {
    //        chargeBar.fillAmount = 1f;
    //        yield break;
    //    }

    //    float elapsed = 0f;
    //    while (elapsed < duration)
    //    {
    //        elapsed += Time.deltaTime;
    //        chargeBar.fillAmount = Mathf.Clamp01(elapsed/duration);

    //        yield return null;

    //        if(!timeCanvas.enabled)
    //            yield break;
    //    }

    //    //Have a delayed time before the player transitions to next scene
    //    yield return new WaitForSeconds(1f);

    //    chargeBar.fillAmount = 0;//Reset timer
    //    timeCanvas.enabled = false;
    //    SceneManager.LoadScene(sceneDestination);
    //}

    public void Death()//Only call this function when player loses all lives
    {
        SceneManager.LoadScene("Town");
    }
}