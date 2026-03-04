using Miyu.Concepts.Resources;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityResourcesBase))]
public class ResourceEffects : MonoBehaviour
{
    public List<ResourceEffect> possibleEffects = new();

    [SerializeField] private List<ResourceEffect> m_InitialEffects = new();

    private readonly Dictionary<EffectType, ResourceModifier> _ActiveEffects = new();
    private EntityResourcesBase m_EntityResources;

    private void Start()
    {
        m_EntityResources = GetComponent<EntityResourcesBase>();
        for (int i = 0; i < m_InitialEffects.Count; i++) ApplyEffect(m_InitialEffects[i]);
    }

    private void Update()
    {
        var keys = new List<EffectType>(_ActiveEffects.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            var modifier = _ActiveEffects[keys[i]];
            modifier.Tick(Time.deltaTime);
            if (modifier.IsFinished) RemoveEffect(keys[i]);
        }
    }

    public void ApplyEffect(ResourceEffect effect)
    {
        if (_ActiveEffects.TryGetValue(effect.effectType, out var existing))
        {
            if (existing.Definition.effectMode == ResourceEffectMode.REFRESH) existing.Refresh();
            return;
        }

        var resource = m_EntityResources.GetResource(effect.resourceTarget);
        var modifier = new ResourceModifier(effect, resource);
        _ActiveEffects.Add(effect.effectType, modifier);
    }

    public void AddEffect(EffectType type)
    {
        var effect = possibleEffects.Find(effectSO => effectSO.effectType == type);
        if (effect == null)
        {
            Debug.LogError($"Effect '{type}' not found in possible effects of {gameObject.name}");
            return;
        }
        ApplyEffect(effect);
    }

    public void RemoveEffect(EffectType type)
    {
        if (_ActiveEffects.TryGetValue(type, out var modifier))
        {
            modifier.Finish();
            _ActiveEffects.Remove(type);
        }
    }

    public void ClearEffects()
    {
        foreach (var modifier in _ActiveEffects.Values) modifier.Finish();
        _ActiveEffects.Clear();
    }

    // DEBUG
    public List<string> GetCurrentEffectNames()
    {
        var names = new List<string>();
        foreach (var modifier in _ActiveEffects.Values) names.Add(modifier.Definition.name);
        return names;
    }
}