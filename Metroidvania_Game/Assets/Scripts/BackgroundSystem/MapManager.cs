using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager intstance;

    [SerializeField] private GameObject miniMap;
    [SerializeField] private GameObject largeMap;

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
    }

    private void CloseLargeMap()
    {
        miniMap.SetActive(true);
        largeMap.SetActive(false);
        isLargeMapOpen=false;
    }
}
