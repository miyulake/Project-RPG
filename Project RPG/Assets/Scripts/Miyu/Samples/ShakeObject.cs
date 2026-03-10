using UnityEngine;
using Miyu.Tools;

namespace Miyu.Samples
{
    public class ShakeObject : MonoBehaviour
    {
        [SerializeField] private Transform m_Visual;
        [Min(0), SerializeField] private float m_Magnitude = 0.25f;
        [Min(0), SerializeField] private float m_Duration = 1f;
        [Min(0), SerializeField] private float m_DecaySpeed = 3f;
        private ObjectShake m_Shake;
        private Vector3 m_BasePosition;
        private Vector3 m_AnchorPosition;

        private void Start()
        {
            if (!m_Visual) m_Visual = transform;

            m_BasePosition = m_Visual.localPosition;
            m_AnchorPosition = m_BasePosition;

            m_Shake = new(m_AnchorPosition);
        }

        private void LateUpdate()
        {
            if (!m_Shake.IsActive) m_BasePosition = m_Visual.localPosition;

            m_Shake.SetStartPosition(m_AnchorPosition);

            var offset = m_Shake.Update(Time.deltaTime);
            m_Visual.localPosition = m_AnchorPosition + offset;
        }

        public void Shake()
        {
            m_AnchorPosition = m_BasePosition;
            m_Shake.SetStartPosition(m_AnchorPosition);
            m_Shake.StartShake(m_Magnitude, m_Duration, m_DecaySpeed);
        }

        public void ShakeExtension(float magnitudeMultiplier = 1, float durationMultiplier = 1)
        {
            m_AnchorPosition = m_BasePosition;
            m_Shake.SetStartPosition(m_AnchorPosition);
            m_Shake.StartShake(m_Magnitude * magnitudeMultiplier, m_Duration * durationMultiplier, m_DecaySpeed);
        }
    }
}