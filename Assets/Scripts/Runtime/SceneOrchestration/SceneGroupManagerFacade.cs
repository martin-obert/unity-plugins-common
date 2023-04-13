using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Obert.Common.Runtime.SceneOrchestration
{
    public sealed class SceneGroupManagerFacade : MonoBehaviour
    {
        [SerializeField] private SceneGroup defaultGroup;

        [SerializeField] private LoadingSceneGroupUnityEvent onSceneLoadingStateChanged;

        [SerializeField] private float debugDelayBetweenOperations;

        private static SceneGroupManager _instance;

        public static ISceneGroupManager Instance => _instance;

        private Action _onDispose;

        private ISceneGroup _currentGroup;

        private void Start()
        {
            if (_instance == null)
            {
                _instance = new SceneGroupManager(debugDelayBetweenOperations);
            }
            else
            {
                Destroy(this);
                return;
            }

#if UNITY_EDITOR

            // We only care about loading the default scene group upon start
            // when there is application entry scene loaded. Otherwise exit
            if (SceneManager.sceneCount != 1) return;

            var initialScene = UnityEditor.EditorBuildSettings.scenes[0];
            var loadedScene = SceneManager.GetSceneAt(0);

            // If the only scene loaded is not application entry point,
            // we don't want to continue loading initial scene group.
            // Since we're in editor, the user could be just developing.
            if (!initialScene.path.Equals(loadedScene.path))
            {
                return;
            }
#endif
            _onDispose = () => _instance = null;
            _instance.SceneLoadingStateChanged += InstanceOnSceneLoadingStateChanged;
            if (defaultGroup)
            {
                LoadGroup(defaultGroup);
            }
        }

        private void InstanceOnSceneLoadingStateChanged(object sender, SceneLoadingState e) => onSceneLoadingStateChanged?.Invoke(e);

        private void OnDestroy()
        {
            _onDispose?.Invoke();
        }

        public void LoadGroup(SceneGroup group) => Instance.LoadGroup(group);
    }
}