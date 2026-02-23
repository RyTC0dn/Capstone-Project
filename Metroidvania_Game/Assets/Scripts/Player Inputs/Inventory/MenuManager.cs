using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// This script is to handle interactions and navigation within each menu page
/// </summary>
public class MenuManager : MonoBehaviour
{
    #region Variables
    [Header("Menu UI Components")]
    [Tooltip("0 is the Shield, 1 equals wall break, 2 is axe")]
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

    public static MenuManager instance { get; private set; }

    [Header("Menu Pages")]
    //Store the separate menu options
    public bool menuOpened = false;
    public GameObject equipmentMenu;
    public GameObject inventoryMenu;
    public GameObject questMenu;
    public GameObject tutorialMenu;
    private PrototypePlayerAttack playerAttack; //So I can disable attack when menu is open
    [Space(20)]

    [Header("Book Icon Indicator1")]
    public GameObject bookIcon;
    public Animator bookAnim;
    [SerializeField]private TextMeshProUGUI bookText;

    [SerializeField] private int sceneIndex;
    #endregion


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        #region Singleton Pattern
        if (instance == null)
            instance = this;

        for (int i = 0; i < itemIcons.Length; i++)
        {
            itemIcons[i].SetActive(false);
        }

        equipmentMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        questMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        #endregion

        bookIcon.SetActive(false);
        playerAttack = FindFirstObjectByType<PrototypePlayerAttack>();
        SceneManager.UnloadSceneAsync(sceneIndex);
    }

    private void Update()
    {
        //If the player presses the inventory menu input
        //TAB - Keyboard or Select - Gamepad
        if (InputManager.Instance.InventoryOpenCloseInput)
        {
            if (menuOpened) //Check if the menu is already open
            {
                CloseMenu();
                //Enable player attack when menu is closed
                if (playerAttack != null)
                    playerAttack.enabled = true;
            }
            else //Otherwise open player menu 
            {
                OpenMenu();
                //Disable player attack when menu is open
                if (playerAttack != null)
                    playerAttack.enabled = false;

                BookIcon(false); //Remove book icon when menu is opened
            }
        }

        //Call Methods
        CheckPickup();
        TrackCoin();
        //BookIcon();
    }

    #region Call Menus
    private void OpenMenu()
    {
        MenuManager.instance.EquipmentOpen();
        //Call the page flipping audio clip
        MenuManager.instance.menuAudio.PlayOneShot
            (MenuManager.instance.menuClips[0]);
        Time.timeScale = 0;
        menuOpened = true;
    }

    private void CloseMenu()
    {
        MenuManager.instance.CloseMenus();
        //Call the close menu audio clip
        MenuManager.instance.menuAudio.PlayOneShot
            (MenuManager.instance.menuClips[1]);
        Time.timeScale = 1f;
        menuOpened = false;
    }
    #endregion

    #region Misc Methods
    private void CheckPickup()
    {
        itemIcons[0].SetActive(sceneInfo.isShieldPickedUp);
        itemIcons[1].SetActive(sceneInfo.isWallBreakPickedUp);
        itemIcons[2].SetActive(sceneInfo.isAxeBought);
    }

    private void TrackCoin()
    {
        //Attach reference from GameManager
        coinTracker = GameManager.instance.currentCoin;
        coinText.text = coinTracker.ToString();
    }

    public void PlayAudio(int index)
    {
        menuAudio.PlayOneShot(menuClips[index]);
    }

    //Call this function when player updates quest or picks up items to show in book 
    private void BookIcon(bool isActive)
    {
        //Animate book depending on whether book menu is needed or not
        bookAnim.SetBool("Active", isActive);

        StartCoroutine(FlashText());

        //Could also add a sound effect here when book icon appears
        //Or a notification icon on the book icon to indicate new content
    }

    private System.Collections.IEnumerator FlashText()
    {
        yield return new WaitForSeconds(0.1f);

        if(bookText != null)
        {
            Color color = bookText.color;
            float elapsed = 0f;

            while(elapsed < 1f)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.PingPong(Time.time * 4f, 0.5f * 5) + 0.5f/2; // oscillates between 0.5–1
                bookText.color = new Color(color.r, color.g, color.b, alpha); //Flashing transparency
                yield return null;
            }

            bookText.color = new Color(color.r, color.g, color.b, 1f);
        }
    }
    #endregion

    #region Menu Tabs
    public void EquipmentOpen()
    {
        equipmentMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(menuFirst[0]);

        inventoryMenu.SetActive(false);
        questMenu.SetActive(false);
        tutorialMenu.SetActive(false);

        menuAudio.PlayOneShot(menuClips[0]);
    }

    public void InventoryOpen()
    {
        equipmentMenu.SetActive(false);
        inventoryMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(menuFirst[1]);

        questMenu.SetActive(false);
        tutorialMenu.SetActive(false);

        menuAudio.PlayOneShot(menuClips[0]);
    }

    public void QuestOpen()
    {
        equipmentMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        questMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(menuFirst[2]);

        tutorialMenu.SetActive(false);

        menuAudio.PlayOneShot(menuClips[0]);
    }

    public void TutorialOpen()
    {
        equipmentMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        questMenu.SetActive(false);
        tutorialMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(menuFirst[3]);

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
        else if(tutorialMenu.activeInHierarchy)
            tutorialMenu.SetActive(false);
        else
        {
            //If no menus are open 
            Debug.LogError("There are no menus open");
            return;
        }
    }
    #endregion
}
