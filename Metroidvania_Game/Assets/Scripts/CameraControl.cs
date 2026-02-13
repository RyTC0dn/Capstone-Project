using UnityEngine;

/// <summary>
/// This script is in need of revision
/// </summary>

public enum Rooms
{
    Entrance,
    Room_A1,
    Room_A2,
    Room_A3,
    Room_A4,
    Room_A5,
    Room_A6,
    Room_A7,
    Tutorial_01,
    Tutorial_02,
    Tutorial_03,
    Tutorial_04,
    Tutorial_05,
    Tutorial_06
}

public class CameraControl : MonoBehaviour
{
    public Transform playerPos;
    public float offsetY;
    public Rooms currentRoom;

    private float minX, maxX, minY, maxY;

    private void LateUpdate()
    {
        if (playerPos == null) return;

        Vector3 pos = new Vector3(playerPos.position.x, playerPos.position.y + offsetY, transform.position.z);

        float clampX = Mathf.Clamp(pos.x, minX, maxX);
        float clampY = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = new Vector3(clampX, clampY, transform.position.z);
    }

    public void SetBounds(float minX, float maxX, float minY, float maxY, Rooms room)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;

        currentRoom = room;
    }
}
