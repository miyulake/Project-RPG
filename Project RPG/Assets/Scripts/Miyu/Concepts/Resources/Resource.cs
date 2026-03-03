using UnityEngine;
using System;

namespace Miyu.Concepts.Resources
{
    public sealed class Resource : IResource
    {
        public int Current { get; private set; }
        public int Max { get; private set; }

        public bool IsEmpty => Current == 0;
        public bool IsFull => Current == Max;

        public event Action<int, int, int> Changed;
        public event Action Emptied;
        public event Action Filled;

        public Resource(int max, int current)
        {
            if (max <= 0) throw new ArgumentOutOfRangeException
                    (nameof(max), "Max value of resource has to be positive!");

            Max = max;
            Current = Math.Clamp(current, 0, max);
        }

        public void ModifyCurrent(int amount) => SetCurrent(Current + amount);

        public void SetCurrent(int value)
        {
            var previous = Current;
            Current = Math.Clamp(value, 0, Max);

            if (Current == previous) return;

            var delta = Current - previous;
            Changed?.Invoke(Current, Max, delta);

            if (Current == 0 && previous > 0) Emptied?.Invoke();
            if (Current == Max && previous < Max) Filled?.Invoke();
        }

        public void ModifyMax(int amount)
        {
            Max += amount;
            Current = Mathf.Clamp(Current, 0, Max);
            Changed?.Invoke(Current, Max, 0);
        }

        public void SetMax(int value)
        {
            Max = value;
            Current = Mathf.Clamp(Current, 0, Max);
            Changed?.Invoke(Current, Max, 0);
        }
    }
}