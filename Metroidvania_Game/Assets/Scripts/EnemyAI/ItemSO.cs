using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableO Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public enum ItemType { gold}
    public ItemType itemType;

    public string itemDescription;
    public string itemName;
    public int itemValue;
}
