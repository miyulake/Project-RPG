using UnityEngine;

namespace Miyu.Concepts.Resources
{
    [CreateAssetMenu(fileName = "NewResourceEffect", menuName = "Miyu/Resources/ResourceEffect")]
    public sealed class ResourceEffect : ScriptableObject
    {
        public EffectType effectType;
        public ResourceEffectMode effectMode;
        public ResourceType resourceTarget;
        [Min(0)] public int duration = 5; // 0 = infinite
        public int baseAmountPerTick = 1;
        [Min(0)] public float intervalSeconds = 1;
        public bool tickImmediately = false;
    }
}