using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public ItemSO item;
    public SceneInfo sceneInfo;
    public GameObject itemPrefab;

    private void Awake()
    {
        itemPrefab.SetActive(false);
        BoughtItem();
    }

    private void Update()
    {
       
    }

    public void BoughtItem()
    {
        if(item != null)
        {
            if (sceneInfo.isHPBought)
            {
                itemPrefab.SetActive(true);
            }
        }
    }
}
