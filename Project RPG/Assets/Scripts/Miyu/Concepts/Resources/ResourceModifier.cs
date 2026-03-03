using Miyu.Tools;

namespace Miyu.Concepts.Resources
{
    public sealed class ResourceModifier
    {
        private readonly ResourceEffectSO _Definition;
        private readonly IResource _Resource;

        private readonly Timer _IntervalTimer;
        private readonly Timer _DurationTimer;

        public ResourceEffectSO Definition => _Definition;
        public bool IsFinished => _DurationTimer != null && _DurationTimer.IsFinished;

        public ResourceModifier(ResourceEffectSO definition, IResource resource)
        {
            _Definition = definition;
            _Resource = resource;

            _IntervalTimer = new Timer(definition.intervalSeconds);

            if (definition.duration > 0f) _DurationTimer = new Timer(definition.duration, true);

            _IntervalTimer.OnCompleted += () =>
            {
                _Resource.Modify(_Definition.baseAmountPerTick);
                _IntervalTimer.Reset();
            };

            if (definition.tickImmediately) _Resource.Modify(_Definition.baseAmountPerTick);
        }

        public void Tick(float deltaTime)
        {
            if (IsFinished) return;

            _DurationTimer?.Tick(deltaTime);

            if (IsFinished) return;

            _IntervalTimer.Tick(deltaTime);
        }
    }
}