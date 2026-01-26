using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class MenuManager : MonoBehaviour
{
    public GameObject[] itemIcons;
    [SerializeField] private SceneInfo sceneInfo;
    [Space(20)]

    [Tooltip("Assign the first button to highlight on each page")]
    public GameObject[] menuFirst; //Highlight first button object for controller navigation
    public AudioSource menuAudio;
    public AudioClip[] menuClips;

    public static MenuManager instance { get; private set; }

    [Header("Menu Pages")]
    //Store the separate menu options
    public GameObject equipmentMenu;
    public GameObject inventoryMenu;
    public GameObject questMenu;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null)
            instance = this;

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
            itemIcons[1].SetActive(false);
        }
    }

    #region Menu Tabs
    public void EquipmentOpen()
    {
        equipmentMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(menuFirst[0]);

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

    public void CloseMenus()
    {
        //Check which menu is currently open
        if (equipmentMenu.activeInHierarchy)
            equipmentMenu.SetActive(false);
        else if(inventoryMenu.activeInHierarchy)
            inventoryMenu.SetActive(false);
        else if(questMenu.activeInHierarchy)
            questMenu.SetActive(false);
        else
        {
            //If no menus are open 
            Debug.LogError("There are no menus open");
            return;
        }
    }
    #endregion
}
