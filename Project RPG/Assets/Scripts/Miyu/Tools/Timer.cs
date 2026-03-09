using System;

namespace Miyu.Tools
{
    public sealed class Timer
    {
        private float m_Duration;
        private float m_Elapsed;

        public bool IsFinished => m_Elapsed >= m_Duration;
        public bool IsRunning => m_Elapsed > 0f && !IsFinished;
        public float Remaining => Math.Max(0f, m_Duration - m_Elapsed);
        public float Progress => Math.Clamp(m_Elapsed / m_Duration, 0f, 1f);

        public event Action OnCompleted;
        public bool Loop { get; set; }

        public float Duration
        {
            get => m_Duration;
            set
            {
                if (value <= 0f) throw new ArgumentOutOfRangeException(nameof(Duration), "Duration must be > 0");
                m_Duration = value;
            }
        }

        public Timer(float duration, bool loop = false)
        {
            Duration = duration;
            m_Elapsed = 0f;
            Loop = loop;
        }


        public float Tick(float deltaTime)
        {
            var leftover = deltaTime;
            while (leftover > 0f)
            {
                if (IsFinished)
                {
                    if (!Loop) break;
                    Reset();
                }

                var timeToFinish = m_Duration - m_Elapsed;

                if (leftover >= timeToFinish)
                {
                    m_Elapsed = m_Duration;
                    leftover -= timeToFinish;
                    OnCompleted?.Invoke();
                    if (Loop) m_Elapsed = 0f;
                }
                else
                {
                    m_Elapsed += leftover;
                    leftover = 0f;
                }
            }

            return leftover;
        }

        public void Reset(float? newDuration = null)
        {
            m_Elapsed = 0f;
            if (newDuration.HasValue) m_Duration = newDuration.Value;
        }

        public void Finish()
        {
            if (IsFinished) return;
            m_Elapsed = m_Duration;
            OnCompleted?.Invoke();
        }
    }
}