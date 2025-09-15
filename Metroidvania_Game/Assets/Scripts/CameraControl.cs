using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform playerPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos = new Vector3(playerPos.position.x, 0, -10);
        transform.position = cameraPos;
    }
}
