using UnityEngine;

public class MiniBossManager : MonoBehaviour
{
    public static MiniBossManager instance;
    private bool canSpawnMiniBoss;
    public bool willSpawnMiniBoss { get => canSpawnMiniBoss; set => canSpawnMiniBoss = value; }

    private void Awake()
    {
        if (instance == null) { instance = this;}
        DontDestroyOnLoad(gameObject);
    }
}
