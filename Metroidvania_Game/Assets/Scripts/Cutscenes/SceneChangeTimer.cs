using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTimer : MonoBehaviour
{
    [Tooltip("Assign the time of the cutscene")]
    public float changeTime;

    [Tooltip("Assign the name of the town or next scene")]
    public string sceneName;

    private void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime <= 0)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}