using UnityEngine;
using UnityEngine.Video;

public class InventoryDescription : MonoBehaviour
{
    public GameObject textDesc;
    public SceneInfo sceneInfo;
    [SerializeField] private VideoClip clip;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textDesc.SetActive(false);
    }

    public void Show()
    {
        textDesc.SetActive(true);
        MenuManager.instance.PlayAudioClip();
    }

    public void Hide()
    {
        textDesc.SetActive(false);
    }
}
