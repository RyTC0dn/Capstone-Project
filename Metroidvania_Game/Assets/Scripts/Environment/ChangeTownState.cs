using UnityEngine;

public class ChangeTownState : MonoBehaviour
{
    public Animator townAnim;
    private bool isBlacksmithSaved;
    private bool isPotionMakerSaved;
    private bool isHealerSaved;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        townAnim.enabled = false;
        isBlacksmithSaved = GameManager.instance.isBlacksmithSaved;
        isPotionMakerSaved = GameManager.instance.isPotionMakerSaved;
        isHealerSaved = GameManager.instance.isHealerSaved;
    }

    // Update is called once per frame
    void Update()
    {
        if(isBlacksmithSaved && gameObject.name == "blacksmith_0")
        {
            townAnim.enabled=true;
        }
    }
}
