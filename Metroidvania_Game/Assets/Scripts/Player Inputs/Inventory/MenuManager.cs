using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// This script is to handle interactions and navigation within each menu page
/// </summary>
public class MenuManager : MonoBehaviour
{
    [Header("Menu UI Components")]
    public GameObject[] itemIcons;
    private int coinTracker;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private SceneInfo sceneInfo;
    [Space(20)]

    [Tooltip("Assign the first button to highlight on each page")]
    public GameObject[] menuFirst; //Highlight first button object for controller navigation

    [Header("Audio")]
    public AudioSource menuAudio;
    public AudioClip[] menuClips;
    [Space(10)]

    [Header("Menu animations")]
    public Animator animator;

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
        TrackCoin();
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
        else //If no items/abilities are picked up
        {
            itemIcons[0].SetActive(false);
            itemIcons[1].SetActive(false);
        }
    }

    private void TrackCoin()
    {
        //Attach reference from GameManager
        coinTracker = GameManager.instance.currentCoin;
        coinText.text = coinTracker.ToString();
    }

    #region Menu Tabs
    public void EquipmentOpen()
    {
        equipmentMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(menuFirst[0]);

        inventoryMenu.SetActive(false);
        questMenu.SetActive(false);

        menuAudio.PlayOneShot(menuClips[0]);
    }

    public void InventoryOpen()
    {
        equipmentMenu.SetActive(false);
        inventoryMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(menuFirst[1]);

        questMenu.SetActive(false);

        menuAudio.PlayOneShot(menuClips[0]);
    }

    public void QuestOpen()
    {
        equipmentMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        questMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(menuFirst[2]);

        menuAudio.PlayOneShot(menuClips[0]);
    }

    public void CloseMenus()
    {
        for (int i = 0; i < menuFirst.Length; i++)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

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
