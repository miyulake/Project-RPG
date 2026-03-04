using Miyu.Concepts.Resources;
using Miyu.Tools;
using UnityEngine;

public class EntityResources : EntityResourcesBase
{
    [SerializeField] private float m_PoiseFillDelay = 5f;
    private Timer m_PoiseFillTimer;

    protected override void Awake()
    {
        base.Awake();

        m_PoiseFillTimer = new(m_PoiseFillDelay);
        m_PoiseFillTimer.OnCompleted += () => { FillResource(ResourceType.POISE); };

        OnResourceChanged += HandleResourceChanged;
    }

    public void HandleResourceChanged(ResourceType type, int current, int max, int delta)
    {
        switch (type)
        {
            case ResourceType.HEALTH:
                if (delta < 0)
                {
                    ModifyResourceCurrent(ResourceType.POISE, -5);
                    m_PoiseFillTimer.Reset();
                }
                if (IsResourceDepleted(type))
                {
                    EffectRunner.ClearEffects();
                    RaiseDeath();
                    Debug.Log($"{gameObject.name} has died!");
                    OnResourceChanged -= HandleResourceChanged;
                }
                break;

            case ResourceType.STAMINA:

                break;

            case ResourceType.MAGIC:

                break;

            case ResourceType.POISE:
                if (IsResourceDepleted(type))
                {
                    RaiseStagger();
                    FillResource(ResourceType.POISE);
                }
                break;
        }
    }

    private void Update()
    {
        m_PoiseFillTimer.Tick(Time.deltaTime);

        // DEBUG
        if (Input.GetKeyDown(KeyCode.F1)) ModifyResourceCurrent(ResourceType.HEALTH, -10);
        if (Input.GetKeyDown(KeyCode.F2)) ModifyResourceCurrent(ResourceType.STAMINA, -5);
        if (Input.GetKeyDown(KeyCode.F3)) ModifyResourceCurrent(ResourceType.MAGIC, -3);

        if (Input.GetKeyDown(KeyCode.F5)) SetResourceMax(ResourceType.HEALTH, GetResource(ResourceType.HEALTH).Max + 10);
        if (Input.GetKeyDown(KeyCode.F6)) EffectRunner.AddEffect(EffectType.POISON);
        if (Input.GetKeyDown(KeyCode.F7)) EffectRunner.ClearEffects();

        if (Input.GetKeyDown(KeyCode.F12)) InitializeResources();
    }
}