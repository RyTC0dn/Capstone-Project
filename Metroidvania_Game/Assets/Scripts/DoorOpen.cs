using UnityEngine;
using UnityEngine.InputSystem;

public class DoorOpen : MonoBehaviour
{
    private Animator doorAnimator;

    public Collider2D leftCollider;
    public Collider2D rightCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorAnimator = GetComponentInParent<Animator>();
    }

    public void OnChildTriggered()
    {
        doorAnimator.SetBool("isOpen", true);
    }

    public void CloseDoor()
    {
        doorAnimator.SetBool("isOpen", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnChildTriggered();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CloseDoor();
        }
    }
}
