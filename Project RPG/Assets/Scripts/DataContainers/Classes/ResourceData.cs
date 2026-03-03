using UnityEngine;
using Miyu.Concepts.Resources;

[System.Serializable]
public class ResourceData
{
    public ResourceType resourceType;
    [Min(1)] public int maxValue = 100;
    public int CurrentValue => maxValue;
}