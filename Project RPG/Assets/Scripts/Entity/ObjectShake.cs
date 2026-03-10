using UnityEngine;
using Miyu.Samples;

namespace Game.Entities.Visuals
{
    public class ObjectShake : ShakeObject
    {
        [Header("Damage References")]
        [Min(0), SerializeField] private int m_MinReferenceDamage = 10;
        [Min(10), SerializeField] private int m_MaxReferenceDamage = 100;

        [Header("Multipliers")]
        [SerializeField] private float m_MinimalMultiplier = 0.1f;
        [SerializeField] private float m_MaxAddedMagnitude = 1f;
        [SerializeField] private float m_MaxAddedDuration = 0.66f;
        private EntityResources m_Resources;

        private void Awake() => m_Resources = GetComponentInParent<EntityResources>();

        private void HandleDamage(int damage)
        {
            var clampedDamage = Mathf.Clamp(-damage, m_MinReferenceDamage, m_MaxReferenceDamage);
            var normalized = (clampedDamage - m_MinReferenceDamage) / (m_MaxReferenceDamage - m_MinReferenceDamage);

            var trauma = 1f - Mathf.Exp(-3f * normalized);

            var magnitude = 1 + Mathf.Lerp(m_MinimalMultiplier, m_MaxAddedMagnitude, trauma);
            var duration = 1 + Mathf.Lerp(m_MinimalMultiplier, m_MaxAddedDuration, trauma);

            ShakeExtension(magnitude, duration);
        }

        private void OnEnable() => m_Resources.OnDamaged += HandleDamage;

        private void OnDisable() => m_Resources.OnDamaged -= HandleDamage;
    }
}
