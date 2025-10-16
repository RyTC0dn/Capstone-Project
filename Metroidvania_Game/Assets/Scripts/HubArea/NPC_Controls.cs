using UnityEngine;

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
    PrototypeShop townShop;

    public GameObject towerUI;
    public GameObject townUI;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        towerSC = GetComponent<SaveSurvivor>();    
        townShop = GetComponent<PrototypeShop>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (states)
        {
            case NPCStates.Tower:
                townUI.SetActive(false);
                townShop.enabled = false;
                towerSC.enabled = true;
                towerUI.SetActive(true);
                break;
            case NPCStates.Town:
                townUI.SetActive(true);
                towerSC.enabled = false;
                towerUI.SetActive(false);
                break;

        }
    }
}
