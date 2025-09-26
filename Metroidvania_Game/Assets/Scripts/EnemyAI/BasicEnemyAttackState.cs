using UnityEngine;

public class BasicEnemyAttackState : MonoBehaviour
{
    public Transform playerPos;
    private Rigidbody2D rb2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackPlayer(Rigidbody2D enemyRigidbody, float speed)
    {
        rb2D = enemyRigidbody;

        Vector2.MoveTowards(enemyRigidbody.position, 
            playerPos.position, speed * Time.deltaTime);
    }
}
