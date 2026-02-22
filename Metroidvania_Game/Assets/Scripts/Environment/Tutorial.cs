using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    [Header("Screen Setup")]
    [SerializeField]private GameObject screen;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField]private GameObject spawn;
    public GameObject player;

    public string sceneToLoad;

    [Header("State")]
    public GameEvent switcthToTutorialState;

    private void Start()
    {
        screen.SetActive(false);
    }

    public void TutorialConfirmation()
    {
        //Play video, then load the tutorial scene
        screen.SetActive(true);
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);   
    }

    public void TransportPlayer()
    {
        SceneManager.LoadScene("CodeTesting");
        player.transform.position = spawn.transform.position;
    }

}
