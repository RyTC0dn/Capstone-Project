using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemSlot[] itemSlots;
    public int gold;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text quantityText;


    private void Start()
    {
        foreach (var slot in itemSlots)
        {
           slot.UpdateUI();
        }
    }

    private void OnEnable()
    {
        Item.OnItemLooted += AddItem;
    }

    private void OnDisable()
    {
        Item.OnItemLooted -= AddItem;
    }

    public void AddItem(ItemSO item, int quantity)
    {
        if(item == null)
        {
            Debug.LogWarning("Inventory add item called with null item");
            return;
        }

        if (item.isGold)
        {
            gold += quantity;
            goldText.text = gold.ToString();
            GameManager.instance.currentCoin = quantity; //Update current coin amount in game manager
            Debug.Log(goldText.text);
            return;
        }
        else
        {
            
            foreach(var slot in itemSlots)
            {
                if(slot.item == null)
                {
                    slot.item = item;
                    slot.quantity = quantity;
                    slot.UpdateUI();
                    return;
                }
            }
        }
    }


}
