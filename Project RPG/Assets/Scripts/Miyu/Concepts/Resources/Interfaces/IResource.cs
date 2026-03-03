using System;

namespace Miyu.Concepts.Resources
{
    public interface IResource
    {
        int Current { get; }
        int Max { get; }

        bool IsEmpty { get; }
        bool IsFull { get; }

        event Action<int, int, int> Changed; // (current, max, delta)
        event Action Emptied;
        event Action Filled;

        void Modify(int amount);
        void Set(int amount);
    }
}