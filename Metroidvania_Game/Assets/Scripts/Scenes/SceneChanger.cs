using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "LevelExit")
        {
            SceneManager.LoadScene(0);
        }

        if (other.tag == "LevelEnter")
        {
            SceneManager.LoadScene(1);
        }
    }
}
