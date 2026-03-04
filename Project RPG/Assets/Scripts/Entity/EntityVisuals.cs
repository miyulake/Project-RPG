using UnityEngine;

public class EntityVisuals : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    private bool m_IsWandering = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4)) 
        {
            m_IsWandering = !m_IsWandering;
            m_Animator.SetBool("IsWandering", m_IsWandering);
        }
    }

    public void PlayDeath() => m_Animator.Play("Death", 0, 0);

    public void PlayStagger() => m_Animator.Play("Stagger", 0, 0);
}