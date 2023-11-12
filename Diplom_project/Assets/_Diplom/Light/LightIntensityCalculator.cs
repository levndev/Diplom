using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityCalculator : MonoBehaviour
{
    [SerializeField] private LightSet lights;
    [SerializeField] private Reference<float> multiplier;
    [SerializeField] private Reference<float> maxValue;

    private float currentIntensity;


    public float CurrentIntensity() => currentIntensity;
    public float MaxIntensity() => maxValue.Value;

    private void Awake()
    {
        currentIntensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentIntensity = 0;
        if (lights == null)
            return;
        foreach (var light in lights)
        {
            if (currentIntensity > maxValue)
                break;

            var intensity = light.intensity;
            var pos = transform.position;
            var lightPos = light.transform.position;
            var direction = (lightPos - pos).normalized;
            var distance = (lightPos - pos).magnitude;

            if (distance <= 0)
            {
                currentIntensity += intensity * multiplier;
                continue;
            }

            if (Physics.Raycast(pos, direction, distance))
                continue;

            var calculatedIntensity = intensity / (4 * Mathf.PI * Mathf.Pow(distance, 2));
            calculatedIntensity *= multiplier;
            if (calculatedIntensity > 100)
            {
                calculatedIntensity = 100;
            }
            currentIntensity += calculatedIntensity;
        }
        if (currentIntensity > maxValue.Value)
            currentIntensity = maxValue;
    }
}
