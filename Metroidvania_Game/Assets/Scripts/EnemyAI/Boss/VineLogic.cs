using UnityEngine;

public class VineLogic : MonoBehaviour
{
    public Vector2[] roomPos;
    private int positionCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        positionCount = Random.Range(0, roomPos.Length);
        transform.position = roomPos[positionCount];
    }

    // Update is called once per frame
    private void Update()
    {
    }
}