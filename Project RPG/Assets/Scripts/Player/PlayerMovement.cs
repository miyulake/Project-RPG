using UnityEngine;

namespace Game.Player.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterController m_CharacterController;
        [SerializeField] private Transform m_Camera;

        [Header("Attributes")]
        [SerializeField] private float m_LookSensitivity = 5;
        [SerializeField] private float m_LookSmoothTime = 0.1f;
        [SerializeField] private float m_YawLookDuration;
        [SerializeField] private float m_LookingUpAmount;
        [SerializeField] private float m_LookingDownAmount;

        [SerializeField] private float m_WalkSpeed = 5f;
        [SerializeField] private float m_RunSpeed = 8f;
        [SerializeField] private float m_Gravity = -9.81f;

        [Header("Ground")]
        [SerializeField] private LayerMask m_GroundCastLayers;
        [SerializeField] private float m_ExtraDistance = 0.1f;
        [SerializeField] private bool m_IsGrounded;

        private Vector3 m_Velocity;
        private float m_CurrentSpeed;
        private float m_BodyTargetYaw;
        private float m_BodyYawVelocity;
        private float m_HeadYawRotation;
        private float m_HeadYawVelocity;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            m_IsGrounded   = IsOnGround(out Ray _, out float _, out float _);
            m_CurrentSpeed = GetSpeed();

            m_CharacterController.Move(GetMovement(Time.deltaTime));
            transform.localRotation = GetBodyRotation(Time.deltaTime);
            m_Camera.localRotation  = GetHeadRotation(Time.deltaTime);
        }

        private bool IsOnGround(out Ray ray, out float radius, out float maxDistance)
        {
            ray = new(transform.position + Vector3.up * (m_CharacterController.radius * 3), -transform.up);
            radius = m_CharacterController.radius;
            maxDistance = m_CharacterController.radius * 2 + m_CharacterController.skinWidth + m_ExtraDistance;

            return Physics.SphereCast(ray, radius, maxDistance, m_GroundCastLayers, QueryTriggerInteraction.Ignore);
        }

        private Vector3 GetMovement(float deltaTime)
        {
            var x = Input.GetAxisRaw("Horizontal");
            var z = Input.GetAxisRaw("Vertical");
            var horizontalMovement = m_CurrentSpeed * (transform.right * x + transform.forward * z).normalized;

            if (m_IsGrounded) m_Velocity.y = 0;
            else m_Velocity.y += m_Gravity * deltaTime;
            
            var finalMovement = new Vector3(horizontalMovement.x, m_Velocity.y, horizontalMovement.z) * deltaTime;
            return finalMovement;
        }

        private Quaternion GetBodyRotation(float deltaTime)
        {
            if (Input.GetKey(KeyCode.E)) m_BodyTargetYaw += m_LookSensitivity;
            if (Input.GetKey(KeyCode.Q)) m_BodyTargetYaw -= m_LookSensitivity;

            var currentYaw = transform.localEulerAngles.y;
            var smoothYaw = Mathf.SmoothDampAngle(currentYaw, m_BodyTargetYaw, ref m_BodyYawVelocity, m_LookSmoothTime, Mathf.Infinity, deltaTime);
            return Quaternion.Euler(transform.localEulerAngles.x, smoothYaw, transform.localEulerAngles.z);
        }

        private Quaternion GetHeadRotation(float deltaTime)
        {
            var currentEulers = m_Camera.localEulerAngles;
            var targetYaw = 0f;

            if (Input.GetKey(KeyCode.R)) targetYaw += m_LookingUpAmount;
            if (Input.GetKey(KeyCode.F)) targetYaw += m_LookingDownAmount;
            m_HeadYawRotation = Mathf.SmoothDamp(m_HeadYawRotation, targetYaw, ref m_HeadYawVelocity, m_YawLookDuration, Mathf.Infinity, deltaTime);

            return Quaternion.Euler(m_HeadYawRotation, currentEulers.y, currentEulers.z);
        }

        private float GetSpeed() => Input.GetKey(KeyCode.LeftShift) ? m_RunSpeed : m_WalkSpeed;

        private void OnDrawGizmosSelected()
        {
            var onGround = IsOnGround(out Ray ray, out float radius, out float maxDistance);
            var endPoint = ray.origin + (ray.direction * maxDistance);

            if (onGround) Gizmos.color = Color.blue;
            else Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(ray.origin, radius);
            Gizmos.DrawWireSphere(endPoint, radius);
            Gizmos.DrawLine(ray.origin, endPoint);
        }
    }
}