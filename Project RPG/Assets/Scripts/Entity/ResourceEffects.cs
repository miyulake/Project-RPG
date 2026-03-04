using Miyu.Concepts.Resources;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityResourcesBase))]
public class ResourceEffects : MonoBehaviour
{
    public List<ResourceEffectSO> possibleEffects = new();

    [SerializeField] private List<ResourceEffectSO> m_InitialEffects = new();

    private readonly List<ResourceModifier> _ActiveEffects = new();
    private readonly HashSet<string> _ActiveEffectNames = new();
    private EntityResourcesBase m_EntityResources;

    private void Start()
    {
        m_EntityResources = GetComponent<EntityResourcesBase>();
        for (int i = 0; i < m_InitialEffects.Count; i++) ApplyEffect(m_InitialEffects[i]);
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;
        for (int i = _ActiveEffects.Count - 1; i >= 0; i--)
        {
            _ActiveEffects[i].Tick(deltaTime);

            if (_ActiveEffects[i].IsFinished)
            {
                _ActiveEffectNames.Remove(_ActiveEffects[i].Definition.name);
                _ActiveEffects.RemoveAt(i);
            }
        }
    }

    public void ApplyEffect(ResourceEffectSO effect)
    {
        if (_ActiveEffectNames.Contains(effect.name)) return;

        var resource = m_EntityResources.GetResource(effect.resourceTarget);
        var modifier = new ResourceModifier(effect, resource);

        _ActiveEffectNames.Add(effect.name);
        _ActiveEffects.Add(modifier);
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
        for (int i = _ActiveEffects.Count - 1; i >= 0; i--)
        {
            if (_ActiveEffects[i].Definition.effectType == type)
            {
                _ActiveEffectNames.Remove(_ActiveEffects[i].Definition.name);
                _ActiveEffects.RemoveAt(i);
            }
        }
    }

    public void ClearEffects()
    {
        for (int i = _ActiveEffects.Count - 1; i >= 0; i--) _ActiveEffects[i].Finish();
    }

    // DEBUG
    public List<string> GetCurrentEffectNames()
    {
        var names = new List<string>();
        for (int i = 0; i < _ActiveEffects.Count; i++) names.Add(_ActiveEffects[i].Definition.name);
        return names;
    }
}