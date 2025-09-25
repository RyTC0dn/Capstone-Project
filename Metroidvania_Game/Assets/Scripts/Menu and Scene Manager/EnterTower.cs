using UnityEngine;

public class EnterTower : MonoBehaviour
{
    SceneController controller;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            controller.EnterTower();
        }
    }
}
