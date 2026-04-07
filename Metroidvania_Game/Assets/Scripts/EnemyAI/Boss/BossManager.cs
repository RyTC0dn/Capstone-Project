using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the boss battle state and controls the display of the demo menu after the boss is defeated.
/// </summary>
/// <remarks>This class handles the transition to the demo menu upon the boss's death, deactivating spikes and
/// pausing the game state. It requires a reference to the scene information and the demo menu GameObject to function
/// correctly.</remarks>
public class BossManager : MonoBehaviour
{
    public GameObject demoMenu;
    public string menuName;
    public SceneInfo sceneInfo;
    public float timeBeforeMenu = 2f;
    public GameObject[] spikes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        demoMenu.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (sceneInfo.isBossKilled)
        {
            StartCoroutine(routine: OnBossDeath());
        }
    }

    private IEnumerator OnBossDeath()
    {
        foreach (GameObject go in spikes)
        {
            go.SetActive(false);
        }

        //Wait for 2 seconds before showing the menu
        yield return new WaitForSeconds(timeBeforeMenu);

        //Switch the game state to pause
        GameManager.instance.StateSwitch(GameStates.Pause);

        //Show the demo menu
        demoMenu.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        sceneInfo.ResetSceneInfo();
        SceneManager.LoadScene(menuName);
    }

    public void Credits()
    {
        sceneInfo.ResetSceneInfo();
        SceneManager.LoadScene("Credits");
    }
}