using UnityEngine;

namespace Miyu.Tools
{
    #region Base
    public abstract class ShakeBase
    {
        protected float m_Magnitude;
        protected float m_Duration;
        protected float m_DecaySpeed;
        protected float m_Elapsed;

        protected Vector3 m_StartPosition;
        protected Vector3 m_CurrentOffset;

        protected const float EPSILON = 0.001f;

        public Vector3 CurrentOffset => m_CurrentOffset;
        public Vector3 CurrentPosition => m_StartPosition + m_CurrentOffset;
        public bool IsActive => m_Elapsed < m_Duration && m_Magnitude > EPSILON;

        protected ShakeBase(Vector3 startPosition) => m_StartPosition = startPosition;

        public virtual void StartShake(float magnitude, float duration, float decaySpeed = 5f)
        {
            m_Magnitude = magnitude;
            m_Duration = duration;
            m_DecaySpeed = decaySpeed;
            m_Elapsed = 0f;
            m_CurrentOffset = Vector3.zero;
        }

        public abstract Vector3 Update(float deltaTime);

        public void SetStartPosition(Vector3 position) => m_StartPosition = position;

        public void Cancel() => m_Elapsed = m_Duration;
    }
    #endregion

    #region Object
    public sealed class ObjectShake : ShakeBase
    {
        public ObjectShake(Vector3 startPosition) : base(startPosition) { }

        public override Vector3 Update(float deltaTime)
        {
            if (!IsActive)
            {
                m_CurrentOffset = Vector3.zero;
                return m_CurrentOffset;
            }

            m_Elapsed += deltaTime;

            var decayFactor = Mathf.Exp(-m_DecaySpeed * m_Elapsed / m_Duration);
            var currentMagnitude = m_Magnitude * decayFactor;
            m_CurrentOffset = Random.insideUnitSphere * currentMagnitude;

            return m_CurrentOffset;
        }
    }
    #endregion

    #region Camera
    public sealed class CameraShake : ShakeBase
    {
        private Vector3 m_TargetOffset;
        private float m_Timer;
        private readonly float _Frequency;

        public CameraShake(Vector3 startPosition, float frequency = 25f) : base(startPosition) 
            => _Frequency = Mathf.Max(0.01f, frequency);

        public override void StartShake(float magnitude, float duration, float decaySpeed = 5f)
        {
            base.StartShake(magnitude, duration, decaySpeed);
            m_TargetOffset = Vector3.zero;
            m_Timer = 0f;
        }

        public override Vector3 Update(float deltaTime)
        {
            if (!IsActive)
            {
                m_CurrentOffset = Vector3.zero;
                return m_CurrentOffset;
            }

            m_Elapsed += deltaTime;

            var decayFactor = Mathf.Exp(-m_DecaySpeed * m_Elapsed / m_Duration);
            var currentMagnitude = m_Magnitude * decayFactor;

            m_Timer += deltaTime;
            var interval = 1f / _Frequency;
            if (m_Timer >= interval)
            {
                m_Timer -= interval;
                m_TargetOffset = Random.insideUnitSphere * currentMagnitude;
            }

            var smoothFactor = _Frequency * 2f * deltaTime;
            m_CurrentOffset = Vector3.Lerp(m_CurrentOffset, m_TargetOffset, smoothFactor);

            return m_CurrentOffset;
        }
    }
    #endregion
}