using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance {  get; private set; }

    [SerializeField]private GameObject screen;
    [SerializeField]private Sprite image;
    [SerializeField] private int sceneIndex;
    private bool readyToStart = false;

    public List<Transform> tutorials = new List<Transform>();

    private void Start()
    {
        screen.SetActive(false);
    }

    public void Show()
    {
        screen.SetActive(true);
        //screen.GetComponent<UnityEngine.UI.Image>().sprite = image;
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
    }

    public void Hide()
    {
        screen.SetActive(false);
        screen.GetComponent<UnityEngine.UI.Image>().sprite = null;
    }

    public void LoadTutorialSpawn(GameObject spawn)
    {
        if (spawn.activeInHierarchy)
        {
            tutorials.Add(spawn.transform);
        }
    }

    public void StartTutorial(GameObject spawnPoint)
    {
        if (tutorials.Count == 0) return;

        if (tutorials.Contains(spawnPoint.transform) && spawnPoint.activeInHierarchy)
        {
            readyToStart = true;
            transform.position = spawnPoint.transform.position;
        }
        else
        {
            //If the scene doesn't have the spawnpoint active and can't be found within the list
            //ensure that this function can't be called
            readyToStart = false;
            return;
        }
    }

}
