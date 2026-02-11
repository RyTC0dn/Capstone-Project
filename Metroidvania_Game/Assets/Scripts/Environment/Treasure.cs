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

    AudioPlayer audioPlayer;

    private Animator animator;
    [SerializeField] private float waitTime;
    private bool isOpened = false;
    [SerializeField]private bool playerDetected = false;
    [Space(20)]

    [Header("Toss Force")]
    [SerializeField] private float timeInAir;
    private float throwForce = 5f;


    private void Start()
    {
        animator = GetComponent<Animator>();
        audioPlayer = FindFirstObjectByType<AudioPlayer>();
        buttonPrompt.SetActive(false);
    }

    private void Update()
    {
        bool keyPressed = Keyboard.current?.eKey.wasPressedThisFrame ?? false;
        bool gamepadPressed = Gamepad.current?.buttonWest.wasPressedThisFrame ?? false;

        if ((keyPressed || gamepadPressed) && playerDetected)
        {
            isOpened = true;
            StartCoroutine(OpenChest());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(isOpened) return;
            playerDetected = true;
            buttonPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpened)
        {
            playerDetected = false;
            if(buttonPrompt != null) buttonPrompt.SetActive(false);
        }
    }

    IEnumerator OpenChest()
    {
        isOpened = true; //Prevent other interactions while opening

        if (animator != null)
        {
            this.animator.SetTrigger("Open");
            Debug.Log("Chest Opening!");
        }
        else
        {
            Debug.LogWarning("Animator component not found on Treasure object.");
        }

        yield return new WaitForSeconds(waitTime);

        audioPlayer.PlayRandomClip(audioPlayer.GetComponent<AudioSource>(), 7, 9); //Play random chest opening sound effect

        if (treasure != null)
        {
            GameObject spawned = Instantiate(treasure, transform.position, Quaternion.identity);

            Rigidbody2D rb = spawned.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                //Randomize direction horizontally and give upward force
                Vector2 direction = new Vector2(Random.Range(-1, 1), 1).normalized;
                rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
            }
            else
            {
                //If the prefab has no rigidbody, give a simple upward offset
                spawned.transform.position += Vector3.up * 0.5f;
            }
        }
        else
        {
            Debug.LogWarning($"Treasure prefab was not assigned to {gameObject.name}");
        }
        if(buttonPrompt != null) buttonPrompt.SetActive(false);
        playerDetected = false;
        Debug.Log("Throw Coins!");
    }
}
