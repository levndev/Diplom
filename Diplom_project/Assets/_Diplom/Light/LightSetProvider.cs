using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSetProvider : MonoBehaviour
{
    [SerializeField] private new Light light;
    [SerializeField] private LightSet set;

    private void OnEnable()
    {
        if (light != null && set != null)
            set.Add(light);
    }

    private void OnDisable()
    {
        if (light != null && set != null)
            set.Remove(light);
    }
}
