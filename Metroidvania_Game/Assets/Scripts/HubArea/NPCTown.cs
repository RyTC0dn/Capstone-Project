using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCTown : MonoBehaviour
{
    [SerializeField]private GameObject buttonPrompt;
    private bool blacksmithSaved, alchemistSaved, healerSaved;
    [SerializeField] private string npcName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Check if the NPCs have been saved from the GameManager
        blacksmithSaved = GameManager.instance.isBlackSmithSaved;
        alchemistSaved = GameManager.instance.isPotionMakerSaved;
        healerSaved = GameManager.instance.isHealerSaved;

        string checkSceneName = SceneManager.GetActiveScene().name;
        if (checkSceneName == "Town")
        {
            if(PlayerPrefs.GetInt("BlackSmithSaved", 0) == 1 && gameObject.name == npcName)
            {
                gameObject.SetActive(blacksmithSaved);
                Debug.Log("Blacksmith saved, NPC active state set to: " + blacksmithSaved);
            }
            else if(PlayerPrefs.GetInt("AlchemistSaved", 0) == 1 && gameObject.name == npcName)
            {
                gameObject.SetActive(alchemistSaved);
                Debug.Log("Alchemist saved, NPC active state set to: " + alchemistSaved);
            }
            else if(PlayerPrefs.GetInt("HealerSaved", 0) == 1 && gameObject.name == npcName)
            {
                gameObject.SetActive(healerSaved);
                Debug.Log("Healer saved, NPC active state set to: " + healerSaved);
            }
        }

        buttonPrompt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            buttonPrompt.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            buttonPrompt.SetActive(false);
        }
    }

}
