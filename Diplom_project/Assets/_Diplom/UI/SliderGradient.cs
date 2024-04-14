using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[ExecuteInEditMode]
public class SliderGradient : MonoBehaviour
{

    [SerializeField] private Gradient gradient = null;
    [SerializeField] private Image image = null;
    [SerializeField] private Slider slider = null;

    private void Update()
    {
        image.color = gradient.Evaluate(slider.value / slider.maxValue);
    }

}
