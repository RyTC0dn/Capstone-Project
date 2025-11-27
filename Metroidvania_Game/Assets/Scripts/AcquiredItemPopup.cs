using TMPro;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class AcquiredItemPopup : MonoBehaviour
{
    public AbilityInformation abilityItem;
    public GameObject pickupObjectCanvas;
    public TextMeshProUGUI itemAbilityName;
    public Image itemAbilityImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemAbilityImage.sprite = abilityItem.abilityImage;
        itemAbilityName.text = abilityItem.abilityName;
        pickupObjectCanvas.SetActive(false);
    }

    public void WhenPickedUp(Component sender, object data)
    {

    }
}
