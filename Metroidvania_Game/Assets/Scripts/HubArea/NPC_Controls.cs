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
    NPCTown townSC;
    PrototypeShop townShop;

    public GameObject towerUI;
    public GameObject townUI;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        towerSC = GetComponent<SaveSurvivor>();    
        townShop = GetComponent<PrototypeShop>();
        townSC = GetComponent<NPCTown>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (states)
        {
            case NPCStates.Tower:
                towerSC.enabled = true;
                townSC.enabled = false;
                break;

        }
    }
}
