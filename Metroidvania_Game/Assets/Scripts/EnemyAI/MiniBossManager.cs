using UnityEngine;

public class MiniBossManager : MonoBehaviour
{
    public static MiniBossManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this;}
        DontDestroyOnLoad(gameObject);
    }
}
