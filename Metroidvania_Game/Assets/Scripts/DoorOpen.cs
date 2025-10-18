using System.Collections;
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
        doorAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        StartCoroutine(CloseDoor());
    }

    public void OnChildTriggered()
    {
        doorAnimator.SetTrigger("DoorOpening");
    }

    private IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(1);
        doorAnimator.SetBool("isNextRoom", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnChildTriggered();
        }
    }
}
