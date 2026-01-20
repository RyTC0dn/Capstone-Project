using UnityEngine;

public class InventoryDescription : MonoBehaviour
{
    public GameObject textDesc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textDesc.SetActive(false);
    }

    public void Show()
    {
        textDesc.SetActive(true);
    }

    public void Hide()
    {
        textDesc.SetActive(false);
    }
}
