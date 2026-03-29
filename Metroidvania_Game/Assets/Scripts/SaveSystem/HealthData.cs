using UnityEngine;

[System.Serializable]
public class HealthData
{
    public int healthdata;

    public HealthData(PlayerHealth health)
    {
        healthdata = health.totalHealth;
    }
}
