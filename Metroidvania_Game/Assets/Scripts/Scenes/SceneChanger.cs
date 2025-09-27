using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && gameObject.tag == "LevelExit")
        {
            SceneManager.LoadScene("Town");
        }

        if (other.tag == "Player" && gameObject.tag == "LevelEnter")
        {
            SceneManager.LoadScene("Level 1 - RyanTestZone");
        }
    }
}
