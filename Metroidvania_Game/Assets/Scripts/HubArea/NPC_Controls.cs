using UnityEngine;
using UnityEngine.SceneManagement;

public enum NPCStates
{
    Tower, 
    Town
}

public class NPC_Controls : MonoBehaviour
{
    [Header("NPC State Settings")]
    public NPCStates states;
    SaveSurvivor towerSC;
    NPCTown townSC;
    PrototypeShop townShop;

    public GameObject towerUI;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        towerSC = GetComponent<SaveSurvivor>();    
        townShop = GetComponent<PrototypeShop>();
        townSC = GetComponent<NPCTown>();
        townSC.enabled = false;
        towerSC.enabled = false;

        string sceneCheckName = SceneManager.GetActiveScene().name;

        if(sceneCheckName == "Town")
        {
            townSC.enabled = true;
            towerSC.enabled = true;
        }
        if(sceneCheckName == "Level 1 - RyanTestZone")
        {
            townSC.enabled = false;
            towerSC.enabled = true;
        }
    }
}
