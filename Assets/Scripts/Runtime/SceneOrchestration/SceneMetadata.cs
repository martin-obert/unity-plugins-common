using Obert.Common.Runtime.Attributes;
using UnityEngine;

namespace Obert.Common.Runtime.SceneOrchestration
{
    [CreateAssetMenu(menuName = "Scene Orchestration/Scene Metadata", fileName = "Scene Metadata", order = 0)]
    public sealed class SceneMetadata : ScriptableObject, ISceneMetadata
    {
        [SerializeField] [SceneRef] private string scenePath;

        [SerializeField] private bool doNotDestroy = false;
        [SerializeField] private bool doNotOverride = false;

        [SerializeField] private bool setSceneActive = false;
        [SerializeField] private string displayName;

        public string DisplayName => displayName;

        public string ScenePath => scenePath;

        public bool DoNotDestroy => doNotDestroy;

        public bool SetSceneActive => setSceneActive;
        public bool DoNotOverride => doNotOverride;
    }
}