using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public enum ItemType { Shield, Charge }
    public ItemType Type;

    public string itemName;
    public string itemDescription;
    public Sprite icon;
    public bool isStackable;
    public int quantity;
}
