using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager intstance;

    [SerializeField] private GameObject miniMap;
    [SerializeField] private GameObject largeMap;
    [SerializeField] private Camera mapCamera;

    public float mapSizeSmall = 20f;
    public float mapSizeLarge = 40f;

    public bool isLargeMapOpen {  get; private set; }


    private void Awake()
    {
        if (intstance == null)
        {
            intstance = this;
        }
        CloseLargeMap();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!isLargeMapOpen)
            {
                OpenLargeMap();
            }
            else
            {
                CloseLargeMap();
            }
        }
    }

    private void OpenLargeMap()
    {
        miniMap.SetActive(false);
        largeMap.SetActive(true);
        isLargeMapOpen = true;
        Time.timeScale = 0f;
        mapCamera.orthographicSize = mapSizeLarge;
    }

    private void CloseLargeMap()
    {
        miniMap.SetActive(true);
        largeMap.SetActive(false);
        isLargeMapOpen=false;
        Time.timeScale = 1.0f;
        mapCamera.orthographicSize = mapSizeSmall;
    }
}
