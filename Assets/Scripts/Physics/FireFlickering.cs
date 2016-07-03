using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class FireFlickering : MonoBehaviour
{
    private float random;
    private Light light;

    public float maxIntensity = 1.35f;
    public float minIntensity = 0.90f;

    void Start()
    {
        light = GetComponentInChildren<Light>();
        random = Random.Range(0.20f, 1.35f);
    }

    void Update()
    {
        FireLightFlickering();
    }

    void FireLightFlickering()
    {
        float lightNoise = Mathf.PerlinNoise(random, Time.time);

        light.intensity = Mathf.Lerp(minIntensity, maxIntensity, lightNoise);
    }
	
}
