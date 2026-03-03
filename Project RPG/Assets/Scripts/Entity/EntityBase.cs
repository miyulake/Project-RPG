using Miyu.Concepts.Resources;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceData
{
    public ResourceType resourceType;
    [Min(1)] public int maxValue = 100;
    public int CurrentValue => maxValue;
}

public abstract class EntityBase : MonoBehaviour
{
    [SerializeField] private EntityType m_EntityType;
    [SerializeField] private List<ResourceData> m_ResourceSetup = new();
    public EffectRunner EffectRunner { get; private set; }

    protected Dictionary<ResourceType, Resource> resources = new();
    public event System.Action<ResourceType, int, int, int> OnResourceChanged;

    protected virtual void Awake()
    {
        EffectRunner = GetComponent<EffectRunner>();

        for (int i = 0; i < m_ResourceSetup.Count; i++)
        {
            var data = m_ResourceSetup[i];
            var resource = new Resource(data.maxValue, data.CurrentValue);
            var type = data.resourceType;

            resource.Changed += (current, max, delta) => 
            { OnResourceChanged?.Invoke(type, current, max, delta); };

            resources[type] = resource;
        }
    }

    public void ModifyResource(ResourceType type, int amount)
    {
        if (resources.TryGetValue(type, out var resource)) resource.Modify(amount);
    }

    public void SetResource(ResourceType type, int amount)
    {
        if (resources.TryGetValue(type, out var resource)) resource.Set(amount);
    }

    public Resource GetResource(ResourceType type)
    {
        resources.TryGetValue(type, out var resource);
        return resource;
    }

    public bool IsResourceDepleted(ResourceType type) => 
        resources.TryGetValue(type, out var resource) && resource.IsEmpty;

    private void Reset()
    {
        var values = (ResourceType[])System.Enum.GetValues(typeof(ResourceType));
        m_ResourceSetup = new List<ResourceData>(values.Length);
        for (int i = 0; i < values.Length; i++)
        {
            m_ResourceSetup.Add(new ResourceData
            { resourceType = values[i], maxValue = 1 });
        }
    }
}