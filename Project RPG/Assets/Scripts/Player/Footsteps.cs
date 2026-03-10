using UnityEngine;
using Miyu.Tools;
using Game.Player.Movement;

public class Footsteps : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement m_Movement;
    [SerializeField] private SurfaceData m_Database;
    [SerializeField] private AudioSource m_Audio;
    [Header("Timer")]
    [SerializeField] private float m_DurationBetween = 1f;
    [SerializeField] private float m_RunTimerMultiplier = 2f;
    [Header("Raycast")]
    [SerializeField] private float m_RaycastDistance = 1.5f;
    private LayerMask m_GroundMask;
    private Timer m_Timer;

    private void Awake()
    {
        var allLayers = ~0;
        m_GroundMask = allLayers;
        m_Timer = new(m_DurationBetween);
    }

    private void Update()
    {
        if (m_Movement.IsMoving())
        {
            var runMultiplier = m_Movement.IsRunning() ? m_RunTimerMultiplier : 1f;
            m_Timer.Tick(Time.deltaTime * runMultiplier);
        }
        if (m_Timer.IsFinished)
        {
            PlayFootstep();
            m_Timer.Reset();
        }
    }

    public void PlayFootstep()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, m_RaycastDistance, m_GroundMask, QueryTriggerInteraction.Ignore))
        {
            var surface = SurfaceDatabase.GetSurface(hit.collider.gameObject.layer);
            var clip = m_Database.GetRandomStep(surface);
            if (clip) m_Audio.PlayOneShot(clip);
            Debug.Log($"Hit surface: {surface}");
        }
    }
}
