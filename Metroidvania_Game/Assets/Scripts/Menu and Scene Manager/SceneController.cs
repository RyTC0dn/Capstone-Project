using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public SceneController instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnterTower()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitTower()
    {
        SceneManager.LoadScene(0);
    }

}
