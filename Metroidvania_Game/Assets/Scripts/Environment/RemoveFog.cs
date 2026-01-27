using UnityEngine;

public class RemoveFog : MonoBehaviour
{
    [SerializeField]
    private GameObject mapFog;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mapFog.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { mapFog.SetActive(false); }
    }
}
