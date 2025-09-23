using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableO Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public enum ItemType { chest}
    public ItemType itemType;

    public GameObject itemDrop;
    public string itemName;
    public int itemValue;
}
