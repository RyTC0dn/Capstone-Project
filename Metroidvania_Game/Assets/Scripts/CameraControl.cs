using UnityEngine;

/// <summary>
/// This script is in need of revision
/// </summary>

public enum Rooms
{
    Town,
    Entrance,
    Room_A1,
    Room_A2,
    Room_A3,
    Room_A4,
    Room_A5,
    Room_A6,
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

    //// Update is called once per frame
    //void Update()
    //{
    //    //Vector3 cameraPos = new Vector3(playerPos.position.x, playerPos.position.y + 2, -10);
    //    //transform.position = cameraPos;

    //    switch (currentRoom)
    //    {
    //        case Rooms.Entrance:
    //            EntranceRoom();
    //            break;
    //        case Rooms.Room_A1:
    //            RoomA1();
    //            break;
    //        case Rooms.Room_A2: 
    //            RoomA2(); 
    //            break;
    //    }
    //}

    //public void EntranceRoom()
    //{
    //    //Setting the boundaries for entrance
    //    minX = -8; maxX = 33.5f;
    //    minY = -1.5f; maxY = 7;

    //    Vector2 clampPos = new Vector2(Mathf.Clamp(transform.position.x, minX, maxX), 
    //        Mathf.Clamp(transform.position.y, minY, maxY));

    //    transform.position = clampPos;
    //}

    //public void RoomA1()
    //{

    //}

    //public void RoomA2()
    //{

    //}
}
