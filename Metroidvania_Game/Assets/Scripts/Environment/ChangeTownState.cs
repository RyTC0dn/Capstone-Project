using UnityEngine;

public class ChangeTownState : MonoBehaviour
{
    public Animator townAnim;
    public GameObject blacksmithHouseBottom;
    public GameObject blacksmithHouseTop;
    public GameObject potionShopBottom;
    private bool isBlacksmithSaved;
    private bool isPotionMakerSaved;
    private bool isHealerSaved;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        townAnim.enabled = false;
        blacksmithHouseBottom.SetActive(false);
        blacksmithHouseTop.SetActive(false);
        potionShopBottom.SetActive(false);
        isBlacksmithSaved = GameManager.instance.isBlackSmithSaved;
        isPotionMakerSaved = GameManager.instance.isPotionMakerSaved;
        isHealerSaved = GameManager.instance.isHealerSaved;
    }

    // Update is called once per frame
    void Update()
    {
        if(isBlacksmithSaved && gameObject.name == "blacksmith_0")
        {
            townAnim.enabled=true;
            blacksmithHouseBottom.SetActive(true);
            blacksmithHouseTop.SetActive(true);
            potionShopBottom.SetActive(true);
        }
        if(isPotionMakerSaved && gameObject.name == "PotionMakerBuildingSheet_0")
        {
            townAnim.enabled=true;
        }
    }
}
