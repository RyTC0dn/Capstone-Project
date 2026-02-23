using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections.Generic;
using UnityEngine.Playables;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance {  get; private set; }

    [SerializeField]private GameObject screen;
    private Sprite image;
    public bool readyToStart = false;
    public Animator animator;
    private float animTime;
    private GameObject player;

    public List<TutorialScene> tutorials = new List<TutorialScene>();

    private void Start()
    {
        screen.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Show(int sceneIndex)
    {
        StartCoroutine(LoadTutorialScene(sceneIndex));
    }

    //Load the tutorial scene additively so that the main scene is still active in the background
    private IEnumerator LoadTutorialScene(int sceneIndex)
    {
        screen.SetActive(true);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        while (!op.isDone)
        {
            yield return null;
        }

        Debug.Log("Tutorial scene loaded");

        Scene tutorialScene = SceneManager.GetSceneByBuildIndex(sceneIndex);    
        SceneManager.SetActiveScene(tutorialScene);
    }

    public void Hide(int sceneIndex)
    {
        screen.SetActive(false);
        SceneManager.UnloadSceneAsync(sceneIndex);
    }

    public void PlayTutorial(int tutorialSceneIndex)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(tutorialSceneIndex));
        Debug.Log("Player moved to tutorial scene");
    }

    //After player is finished with the tutorial, 
    public void EndTutorial(int lastSceneIndex)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(lastSceneIndex));
        Debug.Log("Player moved back to main scene");
    }
}
