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
        //Check if the NPCs have been saved from the GameManager or
        //PlayerPrefs and set the active state of the NPC accordingly
        blacksmithSaved = GameManager.instance != null && GameManager.instance.isBlackSmithSaved 
            || PlayerPrefs.GetInt("BlacksmithSaved", 0) == 1;
        alchemistSaved = GameManager.instance != null && GameManager.instance.isPotionMakerSaved
            || PlayerPrefs.GetInt("AlchemistSaved", 0) == 1;
        healerSaved = GameManager.instance != null && GameManager.instance.isHealerSaved
            || PlayerPrefs.GetInt("HealerSaved", 0) == 1;

        string checkSceneName = SceneManager.GetActiveScene().name;
        if (checkSceneName == "Town")
        {
            if(npcName == "Blacksmith")
            {
                gameObject.SetActive(blacksmithSaved);
                Debug.Log("Blacksmith saved, NPC active state set to: " + blacksmithSaved);
            }
            else if(npcName == "Alchemist")
            {
                gameObject.SetActive(alchemistSaved);
                Debug.Log("Alchemist saved, NPC active state set to: " + alchemistSaved);
            }
            else if(npcName == "Healer")
            {
                gameObject.SetActive(healerSaved);
                Debug.Log("Healer saved, NPC active state set to: " + healerSaved);
            }
            else
            {
                gameObject.SetActive(false);
                Debug.Log("NPC not saved, NPC active state set to: " + gameObject.activeSelf);

            }
        }
        if(buttonPrompt != null)
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
