using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public string sceneDestination;
    [SerializeField]private string spawnPointDestination;
    [SerializeField]private float transportTimer;
    [SerializeField]private Image chargeBar;
    [SerializeField]private Image buttonPrompt;
    private Canvas timeCanvas;

    private void Awake()
    {
        timeCanvas = GameObject.Find("TimerCanvas").GetComponent<Canvas>();
        chargeBar = GameObject.Find("TimerFill").GetComponent<Image>();

        timeCanvas.enabled = false;
        buttonPrompt.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool keyInput = Keyboard.current?.eKey.isPressed ?? false;
        bool buttonInput = Gamepad.current?.xButton.isPressed ?? false;

        if (other.tag == "Player" && gameObject.tag == "LevelExit")
        {
            buttonPrompt.enabled = true;
            //StartCoroutine(TimeToChange(transportTimer));
            if (keyInput || buttonInput)
            {
                SceneManager.LoadScene(sceneDestination);
            }
        }

        if (other.tag == "Player" && gameObject.tag == "LevelEnter")
        {
            buttonPrompt.enabled = true;
            //StartCoroutine(TimeToChange(transportTimer));
            if (keyInput || buttonInput)
            {
                GameManager.instance.nextSpawnPointName = spawnPointDestination;
                SceneManager.LoadScene(sceneDestination);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //chargeBar.fillAmount = 0;
            //timeCanvas.enabled = false;
            buttonPrompt.enabled = false;

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
