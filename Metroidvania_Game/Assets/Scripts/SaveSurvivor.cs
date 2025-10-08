using UnityEngine;
using UnityEngine.InputSystem;

public class SaveSurvivor : MonoBehaviour
{
    [SerializeField]private bool playerIsNear = false;

    public NPC npc;
    public Dialogue conversation;

    private SpriteRenderer npcSP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcSP = GetComponent<SpriteRenderer>();
        npcSP.sprite = npc.npcSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame && playerIsNear)
        {
            Debug.Log("Rescued Survivor!");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {       

        if (collision.CompareTag("Player"))
        {
            playerIsNear = true;
           
        }
    }
}
