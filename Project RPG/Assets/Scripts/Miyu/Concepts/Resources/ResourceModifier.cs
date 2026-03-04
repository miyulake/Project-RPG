using Miyu.Tools;

namespace Miyu.Concepts.Resources
{
    public sealed class ResourceModifier
    {
        private readonly ResourceEffect _Definition;
        private readonly IResource _Resource;

        private readonly Timer _IntervalTimer;
        private readonly Timer _DurationTimer;

        public ResourceEffect Definition => _Definition;
        public bool IsFinished => (_DurationTimer != null && _DurationTimer.IsFinished)  || m_IsManuallyFinished;

        private bool m_IsManuallyFinished = false;

        public ResourceModifier(ResourceEffect definition, IResource resource)
        {
            _Definition = definition;
            _Resource = resource;

            _IntervalTimer = new (definition.intervalSeconds);

            if (definition.duration > 0f) _DurationTimer = new (definition.duration);

            _IntervalTimer.OnCompleted += () =>
                {
                    _Resource.ModifyCurrent(_Definition.baseAmountPerTick);
                    _IntervalTimer.Reset();
                };

            if (definition.tickImmediately) _Resource.ModifyCurrent(_Definition.baseAmountPerTick);
        }

        public void Tick(float deltaTime)
        {
            if (IsFinished) return;

            _DurationTimer?.Tick(deltaTime);

            if (IsFinished) return;

            _IntervalTimer.Tick(deltaTime);
        }

        public void Refresh() => _DurationTimer.Reset();

        public void Finish() => m_IsManuallyFinished = true;
    }
}