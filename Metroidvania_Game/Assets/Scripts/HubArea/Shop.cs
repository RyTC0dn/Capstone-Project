using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject weaponItem;
    private Collider2D itemCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemCollider = GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AvailableItem()
    {
        int g = 1;
        float a = 0.5f;
        Color highlight = Color.green;
    }
}
