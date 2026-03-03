using UnityEngine;
using System.Collections.Generic;
using Miyu.Concepts.Resources;

[RequireComponent(typeof(EntityBase))]
public class ResourceEffectRunner : MonoBehaviour
{
    public List<ResourceEffectSO> possibleEffects = new();

    [SerializeField] private List<ResourceEffectSO> m_InitialEffects = new();

    private readonly List<ResourceModifier> _ActiveEffects = new();
    private readonly HashSet<string> _ActiveEffectNames = new();
    private EntityBase m_Entity;

    private void Start()
    {
        m_Entity = GetComponent<EntityBase>();
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
                _ActiveEffectNames.Remove(_ActiveEffects[i].Definition.effectName);
                _ActiveEffects.RemoveAt(i);
            }
        }
    }

    public void ApplyEffect(ResourceEffectSO effect)
    {
        if (_ActiveEffectNames.Contains(effect.effectName)) return;

        var resource = m_Entity.GetResource(effect.resourceTarget);
        var modifier = new ResourceModifier(effect, resource);

        _ActiveEffects.Add(modifier);
        _ActiveEffectNames.Add(effect.effectName);
    }

    public void AddEffect(string effectName)
    {
        var effect = possibleEffects.Find(effectSO => effectSO.effectName == effectName);
        if (effect == null)
        {
            Debug.LogError($"Effect '{effectName}' not found in possible effects.");
            return;
        }
        ApplyEffect(effect);
    }

    public void RemoveEffect(string effectName)
    {
        for (int i = _ActiveEffects.Count - 1; i >= 0; i--)
        {
            if (_ActiveEffects[i].Definition.effectName == effectName)
            {
                _ActiveEffects.RemoveAt(i);
                _ActiveEffectNames.Remove(effectName);
            }
        }
    }

    public void ClearEffects()
    {
        _ActiveEffects.Clear();
        _ActiveEffectNames.Clear();
    }

    // DEBUG
    public List<string> GetCurrentEffectNames()
    {
        var names = new List<string>();
        for (int i = 0; i < _ActiveEffects.Count; i++) names.Add(_ActiveEffects[i].Definition.effectName);
        return names;
    }
}