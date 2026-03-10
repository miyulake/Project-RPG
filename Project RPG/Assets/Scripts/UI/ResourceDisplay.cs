using UnityEngine;
using UnityEngine.UI;
using Miyu.Concepts.Resources;
using System.Collections;

[System.Serializable]
public class ResourceBar
{
    public ResourceType targetResource;
    public Slider slider;
    public Image fill;
    [HideInInspector] public float velocity;
    public float TargetValue { get; set; }
}
public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] private EntityResources m_Resources;
    [SerializeField] private float m_BarSmoothTime = 0.25f;
    [SerializeField] private ResourceBar[] ResourceBars;

    private void Start()
    {
        m_Resources.OnResourceChanged += OnResourceChanged;

        foreach (var bar in ResourceBars)
        {
            var resource = m_Resources.GetResource(bar.targetResource);
            if (resource != null && bar.slider != null)
            {
                bar.slider.minValue = 0f;
                bar.slider.maxValue = 1f;
                bar.TargetValue = (float)resource.Current / resource.Max;
                //bar.slider.value = bar.TargetValue;
            }
        }
        StartCoroutine(AnimateBars());
    }

    private void OnDisable() => m_Resources.OnResourceChanged -= OnResourceChanged;

    private void OnResourceChanged(ResourceType type, int current, int max, int delta)
    {
        foreach (var bar in ResourceBars)
        {
            if (bar.targetResource == type)
                bar.TargetValue = max > 0 ? (float)current / max : 0f;
        }
    }

    private IEnumerator AnimateBars()
    {
        while (true)
        {
            foreach (var bar in ResourceBars)
            {
                bar.slider.value = Mathf.SmoothDamp(bar.slider.value, bar.TargetValue, ref bar.velocity, m_BarSmoothTime);
            }
            yield return null;
        }
    }
}
