using Miyu.Concepts.Resources;
using TMPro;
using UnityEngine;

public class EntityUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_ResourceUI;
    [SerializeField] private TextMeshProUGUI m_EffectsUI;
    private EntityResources m_EntityResources;

    private void Awake() => m_EntityResources = GetComponent<EntityResources>();

    private void Start() => UpdateUI();

    public void UpdateUI()
    {
        var health =
            $"<color=red>Health</color>: {m_EntityResources.GetResource(ResourceType.HEALTH).Current}/{m_EntityResources.GetResource(ResourceType.HEALTH).Max}\n";
        var stamina =
            $"<color=green>Stamina</color>: {m_EntityResources.GetResource(ResourceType.STAMINA).Current}/{m_EntityResources.GetResource(ResourceType.STAMINA).Max}\n";
        var magic =
            $"<color=purple>Magic</color>: {m_EntityResources.GetResource(ResourceType.MAGIC).Current}/{m_EntityResources.GetResource(ResourceType.MAGIC).Max}\n";
        var poise =
            $"<color=#008080ff>Poise</color>: {m_EntityResources.GetResource(ResourceType.POISE).Current}/{m_EntityResources.GetResource(ResourceType.POISE).Max}";

        m_ResourceUI.text = $"{health}{stamina}{magic}{poise}";
        m_EffectsUI.text = m_EffectsUI.text = $"<color=yellow>Effects</color>: {string.Join(" | ", m_EntityResources.EffectRunner.GetCurrentEffectNames())}";
    }
}
