using System;

namespace Obert.Common.Runtime.SceneOrchestration
{
    public interface ISceneGroupManager
    {
        event EventHandler<SceneLoadingState> SceneLoadingStateChanged; 
        void LoadGroup(ISceneGroup group);
    }
}