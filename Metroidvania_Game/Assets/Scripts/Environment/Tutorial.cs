using UnityEngine;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    [SerializeField]private GameObject screen;
    [SerializeField]private Sprite image;
    [SerializeField] private VideoPlayer videoPlayer;

    private void Start()
    {
        screen.SetActive(false);
    }

    public void Show()
    {
        screen.SetActive(true);
        screen.GetComponent<UnityEngine.UI.Image>().sprite = image;
    }

    public void Hide()
    {
        screen.SetActive(false);
        screen.GetComponent<UnityEngine.UI.Image>().sprite = null;
    }

}
