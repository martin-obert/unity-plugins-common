using System;
using UnityEngine.Events;

namespace Obert.Common.Runtime.SceneOrchestration
{
    [Serializable]
    public sealed class LoadingSceneGroupUnityEvent : UnityEvent<SceneLoadingState>
    {
    }
}