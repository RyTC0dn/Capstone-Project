using UnityEngine;

public class ExitTower : MonoBehaviour
{
    SceneController controller;

    private void Start()
    {
        controller = FindFirstObjectByType<SceneController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            controller.ExitTower();
        }
    }
}
