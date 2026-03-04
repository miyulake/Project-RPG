using Miyu.Concepts.Resources;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private EffectType m_EffectType;
    [SerializeField] private ResourceType m_TargetResource;
    [SerializeField] private int m_Amount;

    private void OnTriggerEnter(Collider col)
    {
        if (!col.TryGetComponent<EntityBase>(out var entity)) return;

        entity.Resources.ModifyResourceCurrent(m_TargetResource, m_Amount);
    }
}