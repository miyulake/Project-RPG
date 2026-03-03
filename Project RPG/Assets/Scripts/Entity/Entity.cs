using Miyu.Concepts.Resources;
using TMPro;
using UnityEngine;

public class Entity : EntityBase
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private TextMeshProUGUI m_ResourceUI;
    [SerializeField] private TextMeshProUGUI m_EffectsUI;
    private bool isWandering = false;

    protected override void Awake()
    {
        base.Awake();
        OnResourceChanged += HandleResourceChanged;
    }

    private void Start() => UpdateUI();

    private void HandleResourceChanged(ResourceType type, int current, int max, int delta)
    {
        UpdateUI();

        switch (type)
        {
            case ResourceType.HEALTH:
                if (delta < 0) ModifyResourceCurrent(ResourceType.POISE, -5);
                if (IsResourceDepleted(type))
                {
                    m_Animator.Play("Death", 0, 0);
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
                    m_Animator.Play("Stagger", 0, 0);
                    SetResourceCurrent(ResourceType.POISE, max);
                }
                break;
        }
    }

    private void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.F1)) ModifyResourceCurrent(ResourceType.HEALTH, -10);
        if (Input.GetKeyDown(KeyCode.F2)) ModifyResourceCurrent(ResourceType.STAMINA, -5);
        if (Input.GetKeyDown(KeyCode.F3)) ModifyResourceCurrent(ResourceType.MAGIC, -3);
        if (Input.GetKeyDown(KeyCode.F4))
        {
            isWandering = !isWandering;
            m_Animator.SetBool("Wander", isWandering);
        }
        if (Input.GetKeyDown(KeyCode.F5)) SetResourceMax(ResourceType.HEALTH, GetResource(ResourceType.HEALTH).Max + 10);
        if (Input.GetKeyDown(KeyCode.F6)) EffectRunner.AddEffect("Poison");
        if (Input.GetKeyDown(KeyCode.F7)) EffectRunner.ClearEffects();
        if (Input.GetKeyDown(KeyCode.F12)) InitializeResources();
    }

    private void UpdateUI()
    {
        m_ResourceUI.text =
            $"<color=red>Health</color>: {GetResource(ResourceType.HEALTH).Current}/{GetResource(ResourceType.HEALTH).Max}\n" +
            $"<color=green>Stamina</color>: {GetResource(ResourceType.STAMINA).Current}/{GetResource(ResourceType.STAMINA).Max}\n" +
            $"<color=purple>Magic</color>: {GetResource(ResourceType.MAGIC).Current}/{GetResource(ResourceType.MAGIC).Max}\n" +
            $"<color=#008080ff>Poise</color>: {GetResource(ResourceType.POISE).Current}/{GetResource(ResourceType.POISE).Max}";

        m_EffectsUI.text = m_EffectsUI.text = $"<color=yellow>Effects</color>: {string.Join(" | ", EffectRunner.GetCurrentEffectNames())}";
    }

    private void OnDestroy() => OnResourceChanged -= HandleResourceChanged;
}