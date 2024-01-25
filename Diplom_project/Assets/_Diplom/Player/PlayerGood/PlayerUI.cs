using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Reference<float> playerLightIntensity;
    [SerializeField] private Reference<float> playerLightIntensityMax;
    [SerializeField] private Slider intensityReadoutSlider;
    [SerializeField] private TextMeshProUGUI intensityReadoutText;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (intensityReadoutSlider != null && playerLightIntensityMax != null)
            intensityReadoutSlider.maxValue = playerLightIntensityMax.Get();
        if (intensityReadoutText != null)
            intensityReadoutText.text = string.Format("{0}", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerLightIntensity != null)
        {
            var value = playerLightIntensity.Get();
            if (intensityReadoutSlider != null)
                intensityReadoutSlider.value = value;
            if (intensityReadoutText != null)
                intensityReadoutText.text = string.Format("{0:f2}", value);
        }
    }
}
