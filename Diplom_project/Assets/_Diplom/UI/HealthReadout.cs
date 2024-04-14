using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthReadout : MonoBehaviour
{
    [SerializeField] private Gradient colorGradient;

    [SerializeField] private Reference<float> currentHealth;
    [SerializeField] private Reference<float> maxHealth;

    [SerializeField] private GameObject healthBarPrefab;

    private RectTransform rectTransform;
    private HorizontalLayoutGroup layoutGroup;

    private List<GameObject> healthBars = new();

    void Regenerate()
    {
        if (healthBars.Count > 0)
        {
            for (int i = 0; i < healthBars.Count; ++i)
            {
                Destroy(healthBars[i]);
            }
        }
        int max = Mathf.RoundToInt(maxHealth.Get());

        float barWidth = (rectTransform.rect.width - layoutGroup.padding.left - layoutGroup.padding.right - (layoutGroup.spacing * (max - 1))) / max;

        for (int i = 0; i < max; ++i)
        {
            var bar = Instantiate(healthBarPrefab, rectTransform);
            bar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barWidth);
            bar.GetComponent<Image>().color = colorGradient.Evaluate((float)i / (float)max);
            healthBars.Add(bar);
        }
        UpdateDisplay();
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        layoutGroup = GetComponent<HorizontalLayoutGroup>();
        //currentHealth.OnChanged += UpdateDisplay;
        maxHealth.OnChanged += Regenerate;
        Regenerate();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        float current = currentHealth.Get();
        float max = maxHealth.Get();
        int threshold = Mathf.CeilToInt(current);
        for (int i = 0; i < healthBars.Count; ++i)
        {
            if (i >= threshold)
                healthBars[i].SetActive(false);
            else
                healthBars[i].SetActive(true);
        }
    }
}
