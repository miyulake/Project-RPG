namespace Miyu.Concepts.Resources
{
    public interface IResourceEffect
    {
        bool IsExpired { get; }
        void Tick(float deltaTime);
        float ModifyRate(float currentRate);
        void Refresh();
    }
}