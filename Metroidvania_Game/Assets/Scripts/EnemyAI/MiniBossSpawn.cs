using UnityEngine;

public class MiniBossSpawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnMiniBoss(MiniBossManager.instance.willSpawnMiniBoss);
    }

    private void SpawnMiniBoss(bool willSpawn)
    {

    }
}
