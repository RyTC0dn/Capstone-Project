using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    public GameObject demoMenu;
    public string menuName;
    public SceneInfo sceneInfo;
    public float timeBeforeMenu = 2f;

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
        //Wait for 2 seconds before showing the menu
        yield return new WaitForSeconds(timeBeforeMenu);

        //Switch the game state to pause
        GameManager.instance.StateSwitch(GameStates.Pause);

        //Show the demo menu
        demoMenu.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(menuName);
    }
}