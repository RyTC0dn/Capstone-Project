using UnityEngine;

public class MiniBossManager : MonoBehaviour
{
    public static MiniBossManager instance;
    private bool canSpawnMiniBoss;
    public bool willSpawnMiniBoss { get => canSpawnMiniBoss; set => canSpawnMiniBoss = value; }

    public SceneInfo SceneInfo;
    public GameObject miniBoss;

    private void Awake()
    {
        if (instance == null) { instance = this;}
        DontDestroyOnLoad(gameObject);
        if (SceneInfo.isMiniBossKilled == true)
        {
            miniBoss.SetActive(false);
        }
    }
}
