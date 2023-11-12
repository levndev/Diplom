using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private LightIntensityCalculator intensityCalculator;
    [SerializeField] private Slider intensityReadout;
    [SerializeField] private Reference<float> intensityThreshold;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (intensityReadout != null && intensityCalculator != null)
        {
            intensityReadout.maxValue = intensityCalculator.MaxIntensity();

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (intensityReadout != null && intensityCalculator != null)
        {
            var value = intensityCalculator.CurrentIntensity();
            intensityReadout.value = value;
            //intensityReadout.SetText(string.Format("{0} Light", intensityCalculator.CurrentIntensity()));
        }
    }
}
