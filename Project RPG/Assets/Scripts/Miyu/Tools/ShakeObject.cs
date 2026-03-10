using UnityEngine;
using Miyu.Tools;

namespace Miyu.Samples
{
    public class ShakeObject : MonoBehaviour
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
        [SerializeField] private Transform m_Visual;
        [SerializeField] private float m_Magnitude = 0.25f;
        [SerializeField] private float m_Duration = 1f;
        [SerializeField] private float m_DecaySpeed = 3f;
        private ObjectShake m_Shake;
        private Vector3 m_BasePosition;
        private Vector3 m_AnchorPosition;

        private void Awake()
        {
            if (!m_Visual) m_Visual = transform;

            m_BasePosition = m_Visual.localPosition;
            m_AnchorPosition = m_BasePosition;

            m_Shake = new(m_AnchorPosition);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_AnchorPosition = m_BasePosition;
                m_Shake.SetStartPosition(m_AnchorPosition);
                m_Shake.StartShake(m_Magnitude, m_Duration, m_DecaySpeed);
            }
        }

        private void LateUpdate()
        {
            if (!m_Shake.IsActive) m_BasePosition = m_Visual.localPosition;

            m_Shake.SetStartPosition(m_AnchorPosition);

            var offset = m_Shake.Update(Time.deltaTime);
            m_Visual.localPosition = m_AnchorPosition + offset;
        }
    }
#endif
}