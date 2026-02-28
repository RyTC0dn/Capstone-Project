using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraZones : MonoBehaviour
{
    [Tooltip("Set what room the boundary is for")]
    public Rooms roomType;
    [Tooltip("Set the boundary the camera moves in")]
    public float minX, minY, maxX, maxY;
    [Tooltip("Set the spawnpoint position within each room")]
    public float spawnPointX, spawnPointY;


    public bool applyToAllCameras = false;
    [Tooltip("If set and apply to all cameras is false, this camera will recieve the bounds")]
    public Camera specificCamera; // If applyToAllCameras is false, specify the camera to apply the bounds to

    public GameObject playerSpawnPoint;
    private GameObject player;

    private Vector2 spawnpointPos;

    private static List<CameraZones> zones = new List<CameraZones>();


    private void Awake()
    {
        //Register this zone in the list on awake
        zones.Add(this);
    }

    private void Start()
    {
        player = GameObject.Find("Character 1");

    }

    private void OnDestroy()
    {
        zones.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            if (applyToAllCameras)
            {
                // FindObjectsOfType returns all active CameraControl instances in the scene
                var allCams = FindObjectsOfType<CameraControl>();
                foreach (var camCrtl in allCams)
                {
                    if (camCrtl != null)
                        camCrtl.SetBounds(minX, maxX, minY, maxY, roomType);
                }
            }
            else
            {
                // Apply to a specific camera if assigned, otherwise fall back to Camera.main
                Camera cam = specificCamera != null ? specificCamera : Camera.main;
                if (cam != null)
                {
                    var camCtrl = cam.GetComponent<CameraControl>();
                    if (camCtrl != null)
                        camCtrl.SetBounds(minX, maxX, minY, maxY, roomType);
                    else
                        Debug.LogWarning($"Camera '{cam.name}' does not have a CameraControl component.");
                }
                else
                {
                    Debug.LogWarning("No camera found to apply bounds to (specificCamera is null and Camera.main is null).");
                }
                SetSpawnPoint();
                ActivateThisSpawn();
            }
            //else if (specificCamera != null)
            //{
            //    CameraControl camControl = specificCamera.GetComponent<CameraControl>();
            //    if (camControl != null)
            //    {
            //        camControl.SetBounds(minX, maxX, minY, maxY, roomType);
            //    }
            //}
            //CameraControl cam = Camera.main.GetComponent<CameraControl>();
            //if (cam != null)
            //{
            //    cam.SetBounds(minX, maxX, minY, maxY, roomType);
            //}
            //SetSpawnPoint();
        }
    }

    private void SetSpawnPoint()
    {
        playerSpawnPoint.transform.position =
                 new Vector2(spawnPointX, spawnPointY);
    }

    private void ActivateThisSpawn()
    {
        foreach (CameraZones zone in zones)
        {
            if (zone.playerSpawnPoint == null) continue;

            if (zone == this)
                zone.playerSpawnPoint.SetActive(true);
            else
                zone.playerSpawnPoint.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        //Drawing rectangle boundary that the camera moves in

        Gizmos.color = new Color(255, 0, 0);

        //Top line
        Vector2 topLeft = new Vector2(minX, maxY);
        Vector2 topRight = new Vector2(maxX, maxY);
        Gizmos.DrawLine(topLeft, topRight);

        //Bottom line
        Vector2 bottomLeft = new Vector2(minX, minY);
        Vector2 bottomRight = new Vector2(maxX, minY);
        Gizmos.DrawLine(bottomLeft, bottomRight);

        //Left line
        Vector2 leftTop = new Vector2(minX, maxY);
        Vector2 leftBottom = new Vector2(minX, minY);
        Gizmos.DrawLine(leftTop, leftBottom);

        //Right line
        Vector2 rightTop = new Vector2(maxX, maxY);
        Vector2 rightBottom = new Vector2(maxX, minY);
        Gizmos.DrawLine(rightTop, rightBottom);

        Gizmos.DrawSphere(new Vector3(spawnPointX, spawnPointY), 0.5f);
    }
}
