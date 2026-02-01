using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum TreasureType
{
    Gold,
    Item,
    Artifact
}

public class Treasure : MonoBehaviour
{
    [Header("Treasure Settings")]
    [SerializeField] private int goldAmount = 100;
    public int GoldAmount => goldAmount;
    public TreasureType treasureType;
    [SerializeField]private GameObject treasure;
    [SerializeField] private GameObject buttonPrompt;

    private Animator animator;
    [SerializeField] private float waitTime;
    private bool isOpened = false;
    private bool playerDetected = false;
    [Space(20)]

    [Header("Toss Force")]
    private Vector2 spawnDirection;
    [SerializeField] private float timeInAir;
    private Rigidbody2D rb;
    private float throwForce = 5f;


    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = treasure.GetComponent<Rigidbody2D>();
        buttonPrompt.SetActive(false);
    }

    private void Update()
    {
        bool keyPressd = Keyboard.current.eKey.wasPressedThisFrame;
        bool gamepadPressed = Gamepad.current?.buttonWest.wasPressedThisFrame ?? false;

        if (playerDetected && !isOpened)
        {
            buttonPrompt.SetActive(true);
            if (keyPressd || gamepadPressed)
            {
                isOpened = true;
                StartCoroutine(OpenChest());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = true;
        }

    }

    IEnumerator OpenChest()
    {
        if(animator != null)
        {
            animator.SetTrigger("Open");
            Debug.Log("Chest Opening!");
        }
        else
        {
            Debug.LogWarning("Animator component not found on Treasure object.");
        }

        yield return new WaitForSeconds(waitTime);

        Instantiate(treasure, transform.position, Quaternion.identity);
        buttonPrompt.SetActive(false);
        Debug.Log("Throw Coins!");
    }

    IEnumerator ThrowCoins()
    {
        rb.AddForce(new Vector2(
            Random.Range(-1f, 1f), 1).normalized * throwForce, 
            ForceMode2D.Impulse);

        yield return new WaitForSeconds(waitTime);

        treasure.transform.position = Vector2.zero;
    }
}
