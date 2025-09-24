using TMPro;
using UnityEngine;

public class BasicEnemyHealth : MonoBehaviour
{
    public int enemyHealth = 2;
    //public TextMeshProUGUI healthTrackerText;
    //public GameObject healthTrackerPrefab;
    private float textOffset = 1;
    private float deathTimer = 1;

    public GameObject coin; //For the prototype, the item drops will be tied to the enemey health

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
            if(coin != null) { Instantiate(coin, transform.position, Quaternion.identity); }
            Destroy(gameObject);

            //IN CASE WE NEED A TIMER TILL DEATH
            //deathTimer -= Time.deltaTime;
            //if (deathTimer < 0) { Destroy(gameObject); }            
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
