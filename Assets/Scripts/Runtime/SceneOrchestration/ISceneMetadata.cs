namespace Obert.Common.Runtime.SceneOrchestration
{
    public interface ISceneMetadata
    {
        string DisplayName { get; }
        string ScenePath { get; }
        bool DoNotDestroy { get; }
        bool SetSceneActive { get; }
        bool IsSingleton { get; }
    }
}