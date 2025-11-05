using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public enum AbilityType { Shield}
    public AbilityType abilityType;

    public string itemName;
    public Sprite icon;

    [Tooltip("Assign a prefab or component holder for the ability")]
    public GameObject abilityPrefab;
}
