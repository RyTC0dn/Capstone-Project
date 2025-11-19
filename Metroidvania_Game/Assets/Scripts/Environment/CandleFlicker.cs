using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CandleFlicker : MonoBehaviour
{
    public Light2D light2D;
    public float intensityMin = 0.8f;
    public float intensityMax = 1.2f;
    public float speed = 2f;

    private float noiseSeed;

    private void Start()
    {
        if (light2D == null)
            light2D = GetComponent<Light2D>();

        noiseSeed = Random.value * 10f;
    }

    private void Update()
    {
        float noise = Mathf.PerlinNoise(noiseSeed, Time.time * speed);
        light2D.intensity = Mathf.Lerp(intensityMin, intensityMax, noise);
    }
}
