using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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
    public TreasureType treasureType;
    [SerializeField]private GameObject treasure;
    [SerializeField] private GameObject buttonPrompt;
    private List<GameObject> spawnedTreasures = new List<GameObject>();

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
        treasure.GetComponent<CoinCollection>().coinType = CoinType.Treasure;
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
            for(int i = 0; i < goldAmount; i++)
            {
                Vector2 spawnPos = (Vector2)transform.position + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1f));

                GameObject spawned = Instantiate(treasure, spawnPos, Quaternion.identity);

                spawnedTreasures.Add(spawned);

                spawned.name = "Coin" + i; //Name the spawned coins for easier debugging
            }
        }
        else
        {
            Debug.LogWarning($"Treasure prefab was not assigned to {gameObject.name}");
        }
        if(buttonPrompt != null) buttonPrompt.SetActive(false);
        playerDetected = false;
    }
}
