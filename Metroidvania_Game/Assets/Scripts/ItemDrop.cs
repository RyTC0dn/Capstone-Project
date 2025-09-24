using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public ItemSO item;

    //Event for currency
    public delegate void GainCurrency();
    public event GainCurrency earnCurrency;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Currency()
    {

    }
}
