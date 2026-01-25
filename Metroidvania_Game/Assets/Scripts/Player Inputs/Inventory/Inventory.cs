using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public class CustomEvent : UnityEvent<GameObject, SceneInfo> { }

public class Inventory : MonoBehaviour
{
    public GameObject[] itemIcons;
    private int itemValues;

    private Dictionary<GameObject, int> items = new Dictionary<GameObject, int>();

    public static Inventory instance { get; private set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        foreach (GameObject item in itemIcons)
        {
            itemValues = item.GetInstanceID();

            items.Add(item, itemValues);
        }

        for (int i = 0; i < itemIcons.Length; i++)
        {
            itemIcons[i].SetActive(false);
        }

    }

    public void Initiate(GameObject icon)
    {
        if (items.ContainsKey(icon))
        {
            for (int i = 0;i < items.Count;i++)
            {
                if (itemIcons[i] == icon)
                {
                    itemIcons[i].SetActive(true);
                }
            }
        }
        
    }
}
