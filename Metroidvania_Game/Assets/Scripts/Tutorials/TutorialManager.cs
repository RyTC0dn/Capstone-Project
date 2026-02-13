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
    [SerializeField] private int sceneIndex;
    public bool readyToStart = false;
    public Animator animator;
    private float animTime;

    public List<TutorialScene> tutorials = new List<TutorialScene>();

    private void Start()
    {
        screen.SetActive(false);
    }

    public void Show()
    {
        screen.SetActive(true);
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
    }

    public void Hide()
    {
        screen.SetActive(false);
        SceneManager.UnloadSceneAsync(sceneIndex);
    }


}
