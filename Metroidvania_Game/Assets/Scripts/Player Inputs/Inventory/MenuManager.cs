using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public class CustomEvent : UnityEvent<GameObject, SceneInfo> { }

public class MenuManager : MonoBehaviour
{
    public GameObject[] itemIcons;
    public SceneInfo sceneInfo;
    private int itemValues;
    [Space(20)]

    private Dictionary<GameObject, int> items = new Dictionary<GameObject, int>();

    public static MenuManager instance { get; private set; }

    [Header("Menu Pages")]
    //Store the separate menu options
    public GameObject equipmentMenu;
    public GameObject inventoryMenu;
    public GameObject questMenu;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < itemIcons.Length; i++)
        {
            itemIcons[i].SetActive(false);
        }

    }

    private void Update()
    {
        CheckPickup();
    }

    private void CheckPickup()
    {
        //Using Scene info scipt object
        //Check for if item is picked up
        if(sceneInfo.isShieldPickedUp)
        {
            itemIcons[0].SetActive(true);
        }
        else if(sceneInfo.isWallBreakPickedUp)
            itemIcons[1].SetActive(true);
        else
        {
            itemIcons[0].SetActive(false);
            itemIcons[1].SetActive(true);
        }
    }
    #region Menu Tabs
    public void EquipmentOpen()
    {
        equipmentMenu.SetActive(true);
        inventoryMenu.SetActive(false);
        questMenu.SetActive(false);
    }

    public void InventoryOpen()
    {
        equipmentMenu.SetActive(false);
        inventoryMenu.SetActive(true);
        questMenu.SetActive(false);
    }

    public void QuestOpen()
    {
        equipmentMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        questMenu.SetActive(true);
    }
    #endregion
}
