using System;

namespace Miyu.Tools
{
    public sealed class Buffer
    {
        private readonly float m_BufferTime;
        private float m_TimeRemaining;

        public bool IsBuffered => m_TimeRemaining > 0f;
        public float TimeRemaining => m_TimeRemaining;
        public float Progress => 1f - (m_TimeRemaining / m_BufferTime);

        public Buffer(float bufferTime)
        {
            if (bufferTime <= 0f) throw new ArgumentOutOfRangeException(nameof(bufferTime), "Buffer time must be > 0");
            m_BufferTime = bufferTime;
        }

        public void BufferInput() => m_TimeRemaining = m_BufferTime;

        public void Tick(float deltaTime)
        {
            if (m_TimeRemaining > 0f) Math.Max(0f, m_TimeRemaining - deltaTime);
        }

        public bool Consume()
        {
            if (!IsBuffered) return false;
            m_TimeRemaining = 0f;
            return true;
        }

        public bool TryConsume(bool condition)
        {
            if (!condition || !IsBuffered) return false;
            m_TimeRemaining = 0f;
            return true;
        }

        public void Reset() => m_TimeRemaining = 0f;
    }
}