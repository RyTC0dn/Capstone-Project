using UnityEngine;

public class NPCTown : MonoBehaviour
{
    public NPC npcData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bool npcSaved = GameManager.instance.IsNPCSaved(npcData.npcName);
        gameObject.SetActive(npcSaved);
    }
}
