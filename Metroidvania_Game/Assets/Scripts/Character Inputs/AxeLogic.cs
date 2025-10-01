using UnityEngine;

public class AxeLogic : MonoBehaviour
{

    [SerializeField]
    float vSpeed = 1.0f;
    [SerializeField]
    float hSpeed = 1.0f;
    [SerializeField]
    Rigidbody2D rb2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D.linearVelocity = (transform.right * vSpeed) + (transform.up *hSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
