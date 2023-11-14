using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private LightIntensityCalculator intensityCalculator;
    [SerializeField] private Slider intensityReadoutSlider;
    [SerializeField] private TextMeshProUGUI intensityReadoutText;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (intensityReadoutSlider != null && intensityCalculator != null)
            intensityReadoutSlider.maxValue = intensityCalculator.MaxIntensity();
        if (intensityReadoutText != null)
            intensityReadoutText.text = string.Format("{0}", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (intensityCalculator != null)
        {
            var value = intensityCalculator.CurrentIntensity();
            if (intensityReadoutSlider != null)
                intensityReadoutSlider.value = value;
            if (intensityReadoutText != null)
                intensityReadoutText.text = string.Format("{0:f2}", value);
        }
    }
}
