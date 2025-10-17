using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCTown : MonoBehaviour
{
    public NPC npcData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bool npcSaved = GameManager.instance.isNPCSaved;
        string checkSceneName = SceneManager.GetActiveScene().name;
        if (checkSceneName == "Town")
        {
            gameObject.SetActive(npcSaved);
        }
        if(checkSceneName == "Level 1 - RyanTestZone")
        {
            gameObject.SetActive(true);
        }
    }
}
