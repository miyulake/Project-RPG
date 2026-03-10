using UnityEngine;
using Miyu.Tools;

namespace Miyu.Samples
{
    public class ShakeCamera : MonoBehaviour
    {
        [Min(0), SerializeField] float m_Magnitude = 0.15f;
        [Min(0), SerializeField] float m_Duration = 1f;
        [Min(0), SerializeField] float m_Frequency = 25f;
        [Min(0), SerializeField] float m_DecaySpeed = 3f;
        private CameraShake m_Shake;
        private Vector3 m_StartPosition;

        private void Start()
        {
            m_StartPosition = transform.localPosition;
            m_Shake = new(m_StartPosition, m_Frequency);
        }

        private void LateUpdate() => transform.localPosition = m_StartPosition + m_Shake.Update(Time.deltaTime);

        public void Shake()
        {
            m_Shake.SetStartPosition(m_StartPosition);
            m_Shake.StartShake(m_Magnitude, m_Duration, m_DecaySpeed);
        }

        public void ShakeExtension(float magnitudeMultiplier = 1, float durationMultiplier = 1)
        {
            m_Shake.SetStartPosition(m_StartPosition);
            m_Shake.StartShake(m_Magnitude * magnitudeMultiplier, m_Duration * durationMultiplier, m_DecaySpeed);
        }
    }
}