using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneDestination;
    [SerializeField]private string spawnPointDestination;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && gameObject.tag == "LevelExit")
        {
            SceneManager.LoadScene(sceneDestination);
        }

        if (other.tag == "Player" && gameObject.tag == "LevelEnter")
        {
            GameManager.instance.nextSpawnPointName = spawnPointDestination;
            SceneManager.LoadScene(sceneDestination);
        }
    }

    ///Ryan's added functions 
    ///
    public void Death()//Only call this function when player loses all lives
    {
        SceneManager.LoadScene("Town");
    }

}
