using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea(2, 5)]
    public string itemDescription;
    public Sprite itemIcon;
    [Space(10)]

    public float health;
    public float speed;
    public float strength;
    public float damage;
    [Space(10)]

    public bool isConsumable;
    public bool isGold;
    public float duration;
}
