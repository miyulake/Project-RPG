using UnityEngine;

public abstract class EntityBase : MonoBehaviour
{
    [SerializeField] private EntityType m_Type;

    public EntityResources Resources { get; private set; }
    public EntityVisuals Visuals { get; private set; }
    public EntityUI UI { get; private set; }

    public EntityType GetEntityType() => m_Type;

    protected virtual void Awake()
    {
        Resources = GetComponent<EntityResources>();
        Visuals = GetComponent<EntityVisuals>();
        UI = GetComponent<EntityUI>();

        Resources.OnResourceChanged += (_, _, _, _) => UI.UpdateUI();
        Resources.OnDeath += Visuals.PlayDeath;
        Resources.OnStagger += Visuals.PlayStagger;
    }

    private void OnDestroy() => Resources.OnResourceChanged -= Resources.HandleResourceChanged;
}