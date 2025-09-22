using TMPro;
using UnityEngine;

public class BasicEnemyHealth : MonoBehaviour
{
    public int enemyHealth = 2;
    //public TextMeshProUGUI healthTrackerText;
    //public GameObject healthTrackerPrefab;
    private float textOffset = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        OnDeath();
        //DisplayHitTracker();
    }

    private void OnDeath()
    {
        if(enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    //private void DisplayHitTracker()
    //{
    //    healthTrackerText.text = enemyHealth.ToString();
    //    healthTrackerPrefab.transform.position = transform.position * textOffset;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            enemyHealth--;
            Debug.Log("Enemy hit!!!");
        }
    }
}
