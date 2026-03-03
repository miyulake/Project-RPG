using UnityEngine;
using System.Collections.Generic;
using Miyu.Concepts.Resources;

public class EffectRunner : MonoBehaviour
{
    [Header("Initial Effects (Inspector)")]
    [SerializeField] private List<ResourceEffectSO> m_Effects = new();
    private readonly List<ResourceModifier> _Active = new();
    private EntityBase m_Entity;

    private void Awake()
    {
        m_Entity = GetComponent<EntityBase>();
        for (int i = 0; i < m_Effects.Count; i++) AddEffect(m_Effects[i]);
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;
        for (int i = _Active.Count - 1; i >= 0; i--)
        {
            _Active[i].Tick(deltaTime);
            if (_Active[i].IsFinished) _Active.RemoveAt(i);
        }
    }

    public void AddEffect(ResourceEffectSO effect) =>
        _Active.Add(new ResourceModifier(effect, m_Entity.GetResource(effect.resourceTarget)));

    // DEBUG
    private readonly List<string> _effectNamesCache = new();
    public List<string> GetCurrentEffectNames()
    {
        _effectNamesCache.Clear();
        for (int i = 0; i < _Active.Count; i++) _effectNamesCache.Add(_Active[i].Definition.name);
        return _effectNamesCache;
    }
}
