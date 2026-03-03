using Miyu.Concepts.Resources;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBase : MonoBehaviour
{
    [SerializeField] private EntityType m_EntityType;
    [SerializeField] private List<ResourceData> m_ResourceSetup = new();
    public ResourceEffectRunner EffectRunner { get; private set; }

    protected Dictionary<ResourceType, Resource> resources = new();
    public event System.Action<ResourceType, int, int, int> OnResourceChanged;

    protected virtual void Awake()
    {
        EffectRunner = GetComponent<ResourceEffectRunner>();
        InitializeResources();
    }

    public Resource GetResource(ResourceType type)
    {
        if (!resources.TryGetValue(type, out var resource))
        {
            Debug.LogError($"Resource {type} not found on {name}");
            return null;
        }
        return resource;
    }

    public void ModifyResourceCurrent(ResourceType type, int amount) => GetResource(type).ModifyCurrent(amount);
    public void SetResourceCurrent(ResourceType type, int amount) =>    GetResource(type).SetCurrent(amount);
    public void ModifyResourceMax(ResourceType type, int amount) =>     GetResource(type).ModifyMax(amount);
    public void SetResourceMax(ResourceType type, int amount) =>        GetResource(type).SetMax(amount);
    public bool IsResourceDepleted(ResourceType type) =>                GetResource(type).IsEmpty;

    public void InitializeResources()
    {
        resources.Clear();
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