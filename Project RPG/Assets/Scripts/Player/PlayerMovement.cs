using UnityEngine;

namespace Game.Player.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerAttributes m_Attributes;
        [SerializeField] private CharacterController m_CharacterController;
        [SerializeField] private Transform m_Camera;
        private Vector3 m_Velocity;
        private float m_CurrentSpeed;
        private float m_BodyTargetYaw;
        private float m_BodyYawVelocity;
        private float m_HeadYawRotation;
        private float m_HeadYawVelocity;
        private bool m_IsGrounded;

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
            maxDistance = m_CharacterController.radius * 2 + m_CharacterController.skinWidth + m_Attributes.extraDistance;

            return Physics.SphereCast(ray, radius, maxDistance, m_Attributes.groundCastLayers, QueryTriggerInteraction.Ignore);
        }

        public bool IsMoving()
            => (Mathf.Abs(m_CharacterController.velocity.x) > 0.01f || Mathf.Abs(m_CharacterController.velocity.z) > 0.01f) && 
            m_IsGrounded;

        public bool IsRunning() => Input.GetKey(KeyCode.LeftShift);

        private Vector3 GetMovement(float deltaTime)
        {
            var x = Input.GetAxisRaw("Horizontal");
            var z = Input.GetAxisRaw("Vertical");
            var horizontalMovement = m_CurrentSpeed * (transform.right * x + transform.forward * z).normalized;

            m_Velocity = new(horizontalMovement.x, m_Velocity.y, horizontalMovement.z);

            if (m_IsGrounded) m_Velocity.y = 0;
            else m_Velocity.y += m_Attributes.gravity * deltaTime;
            
            var finalMovement = new Vector3(horizontalMovement.x, m_Velocity.y, horizontalMovement.z) * deltaTime;
            return finalMovement;
        }

        private Quaternion GetBodyRotation(float deltaTime)
        {
            if (Input.GetKey(KeyCode.E)) m_BodyTargetYaw += m_Attributes.lookSensitivity * deltaTime;
            if (Input.GetKey(KeyCode.Q)) m_BodyTargetYaw -= m_Attributes.lookSensitivity * deltaTime;

            var currentYaw = transform.localEulerAngles.y;
            var smoothYaw = Mathf.SmoothDampAngle(currentYaw, m_BodyTargetYaw, ref m_BodyYawVelocity, m_Attributes.lookSmoothTime, Mathf.Infinity, deltaTime);
            return Quaternion.Euler(transform.localEulerAngles.x, smoothYaw, transform.localEulerAngles.z);
        }

        private Quaternion GetHeadRotation(float deltaTime)
        {
            var currentEulers = m_Camera.localEulerAngles;
            var targetYaw = 0f;

            if (Input.GetKey(KeyCode.R)) targetYaw += m_Attributes.lookingUpAmount;
            if (Input.GetKey(KeyCode.F)) targetYaw += m_Attributes.lookingDownAmount;
            m_HeadYawRotation = Mathf.SmoothDamp(m_HeadYawRotation, targetYaw, ref m_HeadYawVelocity, m_Attributes.yawLookDuration, Mathf.Infinity, deltaTime);

            return Quaternion.Euler(m_HeadYawRotation, currentEulers.y, currentEulers.z);
        }

        private float GetSpeed() => Input.GetKey(KeyCode.LeftShift) ? m_Attributes.runSpeed : m_Attributes.walkSpeed;

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