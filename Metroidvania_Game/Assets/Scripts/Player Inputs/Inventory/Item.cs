using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO item;
    [SerializeField]private SpriteRenderer sr;
    public int quantity;
    public static event Action<ItemSO, int> OnItemLooted;
 
    private void OnValidate()
    {
        if (item == null)
            return;

        sr.sprite = item.itemIcon;
        this.name = item.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnItemLooted?.Invoke(item, quantity);
            Destroy(gameObject, 0.5f);
        }
    }
}
