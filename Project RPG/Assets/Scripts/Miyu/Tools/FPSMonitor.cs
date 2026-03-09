using System;

namespace Miyu.Tools
{
    public sealed class FPSMonitor
    {
        private readonly float m_UpdateInterval;

        private float m_AccumulatedTime;
        private int m_FrameCount;

        public float AverageFPS { get; private set; }

        public event Action<float> OnUpdated;

        public FPSMonitor(float updateInterval = 0.33f)
        {
            if (updateInterval <= 0f)
                throw new ArgumentOutOfRangeException(nameof(updateInterval));

            m_UpdateInterval = updateInterval;
        }

        public void Tick(float deltaTime)
        {
            m_AccumulatedTime += deltaTime;
            m_FrameCount++;

            if (m_AccumulatedTime >= m_UpdateInterval)
            {
                AverageFPS = m_FrameCount / m_AccumulatedTime;

                OnUpdated?.Invoke(AverageFPS);

                m_AccumulatedTime = 0f;
                m_FrameCount = 0;
            }
        }

        public void Reset()
        {
            m_AccumulatedTime = 0f;
            m_FrameCount = 0;
            AverageFPS = 0f;
        }
    }
}