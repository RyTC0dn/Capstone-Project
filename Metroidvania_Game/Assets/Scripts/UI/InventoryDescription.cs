using UnityEngine;
using UnityEngine.Video;

public class InventoryDescription : MonoBehaviour
{
    public GameObject textDesc;
    public SceneInfo sceneInfo;
    [SerializeField] private GameObject video;
    [SerializeField] private VideoPlayer player;
    [SerializeField] private VideoClip clip;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textDesc.SetActive(false);
        video.SetActive(false);       
    }

    public void Show()
    {
        textDesc.SetActive(true);
        video.SetActive(true);
        player.clip = clip;
        player.Play();
    }

    public void Hide()
    {
        textDesc.SetActive(false);
        video.SetActive(false);
        player.Stop();
    }
}
