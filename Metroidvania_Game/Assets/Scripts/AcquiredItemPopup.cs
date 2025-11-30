using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AcquiredItemPopup : MonoBehaviour
{
    public AbilityInformation abilityInfo;
    [Space(20)]

    public TextMeshProUGUI itemText;
    public GameObject uiCanvas;
    public Image abilityImage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiCanvas.SetActive(false);
        itemText.text = abilityInfo.abilityName;
        abilityImage.sprite = abilityInfo.abilityImage;
    }

    public void OnAbilityPickup(Component sender, object data)
    {
        if(sender is ItemPickup && sender.gameObject == this.gameObject)
        {
            uiCanvas.SetActive(true);
            GameManager.instance.StateSwitch(GameStates.Pause);
        }
    }

    public void Confirm()
    {
        GameManager.instance.StateSwitch(GameStates.Play);
        uiCanvas.SetActive(false);
        Destroy(gameObject, 0.5f);
    }


}
