using System;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Obert.Common.Runtime.Extensions;
using UnityEngine.SceneManagement;

namespace Obert.Common.Runtime.SceneOrchestration
{
    public sealed class SceneGroupManager : ISceneGroupManager, IDisposable
    {
        private ISceneGroup _currentGroup;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly float _debugDelayBetweenOperations;
        public bool IsLoading { get; private set; }
        public event EventHandler<SceneLoadingState> SceneLoadingStateChanged;

        public SceneGroupManager( float debugDelayBetweenOperations = 0)
        {
            _debugDelayBetweenOperations = debugDelayBetweenOperations;
        }

        public void LoadGroup(ISceneGroup group)
        {
            if (IsLoading) throw new Exception("Another group is already loading. Unable to start new group loading");
            UniTask.Void(async t => await LoadGroupAsync(group, t), _cancellationTokenSource.Token);
        }

        private async UniTask LoadGroupAsync(ISceneGroup group, CancellationToken cancellationToken = default)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            group.Items.ThrowIfEmptyOrNull();


            if (IsLoading) return;

            IsLoading = true;

            var oneStep = 1f / ((_currentGroup?.Items?.Length ?? 0) + group.Items.Length);
            var sceneLoadingState = new SceneLoadingProgressHandle(oneStep);

            OnSceneLoadingStateChanged(sceneLoadingState);

            sceneLoadingState.OnProgress?.Invoke(0);

            if (_currentGroup != null)
            {
                await UnloadGroup(_currentGroup, sceneLoadingState, cancellationToken);
            }

            var loadedScenes = _currentGroup?.Items ?? Array.Empty<SceneMetadata>();

            foreach (var sceneGroupItem in group.Items)
            {
                if (sceneGroupItem.IsSingleton && loadedScenes.Any(x => x.ScenePath.Equals(sceneGroupItem.ScenePath)))
                {
                    sceneLoadingState.ProgressIn();
                    continue;
                }

                var asyncOperation = SceneManager.LoadSceneAsync(sceneGroupItem.ScenePath, LoadSceneMode.Additive);
                await asyncOperation;

                if (_debugDelayBetweenOperations > 0)
                    await UniTask.Delay(TimeSpan.FromSeconds(_debugDelayBetweenOperations),
                        cancellationToken: cancellationToken);

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

        private async UniTask UnloadGroup(ISceneGroup groupToUnload,
            SceneLoadingProgressHandle sceneLoadingState, CancellationToken cancellationToken)
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

                await asyncOperation;

                if (_debugDelayBetweenOperations > 0)
                    await UniTask.Delay(TimeSpan.FromSeconds(_debugDelayBetweenOperations),
                        cancellationToken: cancellationToken);

                sceneLoadingState.ProgressIn();
            }
        }

        private void OnSceneLoadingStateChanged(SceneLoadingState e)
        {
            SceneLoadingStateChanged?.Invoke(this, e);
        }

        public void Dispose()
        {
            if (_cancellationTokenSource is { IsCancellationRequested: false })
                _cancellationTokenSource.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}