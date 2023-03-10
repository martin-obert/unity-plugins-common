using System;
using System.Collections;
using System.IO;
using System.Linq;
using Obert.Common.Runtime.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Obert.Common.Runtime.SceneOrchestration
{
    public class SceneGroupManager : MonoBehaviour, ISceneGroupManager
    {
        [SerializeField] private SceneGroup defaultGroup;

        [SerializeField] private LoadingSceneGroupUnityEvent onLoadingStarted;

        [SerializeField] private float debugDelayBetweenOperations;

        public bool IsLoading { get; private set; }

        public static ISceneGroupManager Instance { get; private set; }

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (!ReferenceEquals(this, Instance))
                {
                    Destroy(this);
                    return;
                }
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
            
            if (defaultGroup)
            {
                LoadGroup(defaultGroup);
            }
        }

        private ISceneGroup _currentGroup;

        public void LoadGroup(ISceneGroup group)
        {
            if (IsLoading) throw new Exception("Another group is already loading. Unable to start new group loading");
            StartCoroutine(LoadGroupAsync(group));
        }

        private IEnumerator LoadGroupAsync(ISceneGroup group)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            group.Items.ThrowIfEmptyOrNull();


            if (IsLoading) yield break;

            IsLoading = true;

            var oneStep = 1f / ((_currentGroup?.Items?.Length ?? 0) + group.Items.Length);
            var sceneLoadingState = new SceneLoadingProgressHandle(oneStep);

            onLoadingStarted.Invoke(sceneLoadingState);

            sceneLoadingState.OnProgress?.Invoke(0);

            if (_currentGroup != null)
            {
                yield return UnloadGroup(_currentGroup, group, sceneLoadingState);
            }

            var loadedScenes = _currentGroup?.Items ?? Array.Empty<SceneMetadata>();

            foreach (var sceneGroupItem in group.Items)
            {
                if (sceneGroupItem.DoNotOverride && loadedScenes.Any(x => x.ScenePath.Equals(sceneGroupItem.ScenePath)))
                {
                    sceneLoadingState.ProgressIn();
                    continue;
                }

                var asyncOperation = SceneManager.LoadSceneAsync(sceneGroupItem.ScenePath, LoadSceneMode.Additive);
                yield return asyncOperation;

                if (debugDelayBetweenOperations > 0)
                    yield return new WaitForSecondsRealtime(debugDelayBetweenOperations);

                if (sceneGroupItem.SetSceneActive)
                {
                    var sceneName = Path.GetFileNameWithoutExtension(sceneGroupItem.ScenePath);
                    SceneManagerExtensions.SetActiveScene(sceneName);
                }

                sceneLoadingState.ProgressIn();
            }

            _currentGroup = group;

            // Keep this as safety, because the progress tracked by state is relatively incremented
            sceneLoadingState.OnProgress?.Invoke(1);

            sceneLoadingState.OnComplete?.Invoke();

            IsLoading = false;
        }

        private IEnumerator UnloadGroup(ISceneGroup groupToUnload, ISceneGroup loadedGroup,
            SceneLoadingProgressHandle sceneLoadingState)
        {
            if (groupToUnload == null) throw new ArgumentNullException(nameof(groupToUnload));

            foreach (var groupItem in groupToUnload.Items)
            {
                // Is already loaded and cannot be overriden
                if (groupItem.DoNotDestroy)
                {
                    sceneLoadingState.ProgressIn();
                    continue;
                }

                var asyncOperation = SceneManager.UnloadSceneAsync(groupItem.ScenePath);

                yield return asyncOperation;

                if (debugDelayBetweenOperations > 0)
                    yield return new WaitForSecondsRealtime(debugDelayBetweenOperations);

                sceneLoadingState.ProgressIn();
            }
        }
    }
}