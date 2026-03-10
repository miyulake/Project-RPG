using UnityEngine;
using Game.Player.Movement;

public class CameraVisuals : MonoBehaviour
{
    [SerializeField] private PlayerMovement m_Movement;
    [SerializeField] private Transform m_CameraHolder;
    private Vector3 m_OriginalPosition;

    [Header("FOV Settings")]
    [SerializeField] private AnimationCurve m_FOVCurve;
    [SerializeField] private float m_FOVCurveDuration = 0.25f;
    [SerializeField] private float m_MainFOV = 75f;
    [SerializeField] private float m_RunFOV = 80f;
    private float m_FOVCurveTime = 0f;

    [Header("Bob Settings")]
    [SerializeField] private float m_BobWalkSpeed = 4f;
    [SerializeField] private float m_BobRunSpeed = 8f;
    [SerializeField] private float m_BobAmount = 0.04f;
    [SerializeField] private float m_BobSmoothDuration = 0.2f;
    private Vector3 m_CurrentBobOffset;
    private Vector3 m_BobVelocity;
    private float m_BobTime = 0f;

    [Header("Tilt Settings")]
    [SerializeField] private float m_WalkTiltSpeed = 8f;
    [SerializeField] private float m_RunTiltSpeed = 16f;
    [SerializeField] private float m_TiltMultiplier = 1f;
    [SerializeField] private float m_TiltSmoothDuration = 0.2f;
    private float m_CurrentTilt = 0f;
    private float m_TiltVelocity = 0f;
    private float m_TiltTime = 0f;

    private Camera MainCamera => Camera.main;

    private void Start() => m_OriginalPosition = m_CameraHolder.localPosition;

    private void Update()
    {
        HandleCameraFOV();
        HandleCameraBob();
        HandleCameraTilt();
    }

    private void HandleCameraFOV()
    {
        var isRunning = m_Movement.IsRunning() && m_Movement.IsMoving();
        var curveDirection = isRunning ? 1f : -1f;

        m_FOVCurveTime = Mathf.Clamp01(m_FOVCurveTime + curveDirection * Time.deltaTime / m_FOVCurveDuration);

        var curveValue = m_FOVCurve.Evaluate(m_FOVCurveTime);
        MainCamera.fieldOfView = Mathf.Lerp(m_MainFOV, m_RunFOV, curveValue);
    }

    private void HandleCameraBob()
    {
        var bobSpeed = m_Movement.IsRunning() ? m_BobRunSpeed : m_BobWalkSpeed;
        m_BobTime += Time.deltaTime * bobSpeed;

        var xOffset = Mathf.Sin(m_BobTime) * m_BobAmount;
        var yOffset = Mathf.Cos(m_BobTime * 2) * m_BobAmount * 0.5f; // Half height for better visual
        Vector3 targetOffset;

        if (m_Movement.IsMoving()) targetOffset = new(xOffset, yOffset);
        else targetOffset = Vector3.zero;

        m_CurrentBobOffset = Vector3.SmoothDamp(m_CurrentBobOffset, targetOffset, ref m_BobVelocity, m_BobSmoothDuration);
        m_CameraHolder.localPosition = m_OriginalPosition + m_CurrentBobOffset;
    }

    private void HandleCameraTilt()
    {
        var targetTilt = 0f;

        if (m_Movement.IsMoving())
        {
            var tiltSpeed = m_Movement.IsRunning() ? m_RunTiltSpeed : m_WalkTiltSpeed;
            m_TiltTime += Time.deltaTime * tiltSpeed;
            targetTilt = Mathf.Sin(m_TiltTime * 0.5f) * m_TiltMultiplier;
        }
        m_CurrentTilt = Mathf.SmoothDampAngle(m_CurrentTilt, targetTilt, ref m_TiltVelocity, m_TiltSmoothDuration);
        //var xTilt = Mathf.Max(0, currentTilt);
        m_CameraHolder.localRotation = Quaternion.Euler(0, 0, m_CurrentTilt);
    }
}
