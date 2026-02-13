using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public ItemSO item;
    public int quantity;

    public Image itemImage;
    public TMP_Text quantityText;

    public void UpdateUI()
    {
        if (item != null)
        {
            itemImage.sprite = item.itemIcon;
            itemImage.gameObject.SetActive(true);
            quantityText.text = quantity.ToString();
        }
        else
        {
            itemImage.gameObject.SetActive(false);
            quantityText.text = "";
        }
    }
}
