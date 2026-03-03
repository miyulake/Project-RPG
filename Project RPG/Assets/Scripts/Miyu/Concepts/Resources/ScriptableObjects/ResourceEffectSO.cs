using UnityEngine;

namespace Miyu.Concepts.Resources
{
    [CreateAssetMenu(fileName = "NewResourceEffect", menuName = "Miyu/Resources/ResourceEffect")]
    public sealed class ResourceEffectSO : ScriptableObject
    {
        public ResourceType resourceTarget;
        public string effectName;
        public int duration = 5; // < 0 > infinite
        public int baseAmountPerTick = 1;
        [Min(0)] public float intervalSeconds = 1;
        public bool tickImmediately = false;
    }
}