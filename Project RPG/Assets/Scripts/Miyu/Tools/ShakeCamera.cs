using UnityEngine;
using Miyu.Tools;

namespace Miyu.Samples
{
    public class ShakeCamera : MonoBehaviour
    {
#if !ENABLE_LEGACY_INPUT_MANAGER
        private void Start()
        {
            Debug.LogWarningFormat(
                this,
                "{0} requires the Legacy Input Manager and will not function.",
                nameof(ShakeCamera));
            enabled = false;
        }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
        [SerializeField] float m_Magnitude = 0.15f;
        [SerializeField] float m_Duration = 1f;
        [SerializeField] float m_Frequency = 25f;
        [SerializeField] float m_DecaySpeed = 3f;
        private CameraShake m_Shake;
        private Vector3 m_StartPosition;

        private void Awake()
        {
            m_StartPosition = transform.localPosition;
            m_Shake = new(m_StartPosition, m_Frequency);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Shake.SetStartPosition(m_StartPosition);
                m_Shake.StartShake(m_Magnitude, m_Duration, m_DecaySpeed);
            }
        }

        private void LateUpdate() => transform.localPosition = m_StartPosition + m_Shake.Update(Time.deltaTime);
    }
#endif
}