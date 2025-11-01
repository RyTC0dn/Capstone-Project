using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
   public Dictionary<ItemSO, Component> abilities = new Dictionary<ItemSO, Component>();
    private bool abilityAdded;
    public int maxSlots = 10;
    public int numberOfItems = 0;

    public delegate void OnInventoryChanged();
    public event OnInventoryChanged onInventoryChanged;

    public delegate void OnAbilityAdded();
    public event OnAbilityAdded onAddAbility;

    public void AddItem(ItemSO newAbility, GameObject player)
    {
        if (newAbility == null || player == null)
        {
            Debug.LogError("Invalid parameters for AddItem");
            return;
        }

        if(abilities.ContainsKey(newAbility))
        {
            Debug.Log($"{newAbility.name} already aquired");
            return;
        }

        //Instantiate or attach ability
        Component abilityComponent = null;
        if(newAbility.abilityPrefab != null)
        {
            GameObject abilityInstance = Instantiate(newAbility.abilityPrefab, player.transform);
            abilityComponent = abilityInstance.GetComponent<MonoBehaviour>();
        }

        //Store the ability reference
        if (abilityComponent != null && numberOfItems < maxSlots)
        {
            abilities.Add(newAbility, abilityComponent);
            numberOfItems++;
            onAddAbility?.Invoke();

        }
        onInventoryChanged?.Invoke();
    }
}
