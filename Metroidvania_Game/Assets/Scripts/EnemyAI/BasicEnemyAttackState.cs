using UnityEngine;

public class BasicEnemyAttackState : MonoBehaviour
{
    public GameObject playerPos;
    private Rigidbody2D rb2D;

    private enum enemyTypes { ground, flying}
    enemyTypes currentEnemyType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentEnemyType)
        {
            case enemyTypes.ground:
                GroundEnemy();
                break;
            case enemyTypes.flying:
                FlyingEnemy();
                break;
        }

        if (gameObject.tag == "GroundEnemy") { currentEnemyType = enemyTypes.ground; }
        if (gameObject.tag == "FlyingEnemy") { currentEnemyType = enemyTypes.flying; }
    }

    public void GroundEnemy()
    {
        float speed = GetComponent<BasicEnemyControls>().enemySpeed;
        Vector2 playerPosX = new Vector2(playerPos.transform.position.x, transform.position.y);
 
        transform.position = Vector2.MoveTowards(rb2D.position, playerPosX, speed * Time.deltaTime);
    }

    public void FlyingEnemy()
    {

    }
}
