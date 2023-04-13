using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Obert.Common.Runtime.Extensions;
using UnityEngine;

namespace Obert.Common.Runtime.Tasks
{
    public sealed class BackgroundTaskRunner : MonoBehaviour, IBackgroundTaskRunner
    {
        private CancellationTokenSource _cancellationTokenSource;

        private IBackgroundTask[] _tasks;

        private Action _onComplete;

        public void SetTasks(IBackgroundTask[] tasks, Action onComplete, CancellationToken cancellationToken = default)
        {
            tasks.ThrowIfEmptyOrNull();
            _onComplete = onComplete;
            this._tasks = tasks;
            if (cancellationToken != default)
            {
                _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            }
        }

        private void Start()
        {
            _tasks.ThrowIfEmptyOrNull();

            _cancellationTokenSource ??= new CancellationTokenSource();
            UniTask.Void(async t =>
                {
                    await UniTask.WhenAll(_tasks.Select(async x =>
                    {
                        try
                        {
                            await x.Execute(t);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }));
                    UniTask.ReturnToMainThread(t);
                    Destroy(this);
                    OnComplete();
                },
                _cancellationTokenSource.Token);
        }


        private void OnDestroy()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource?.Cancel();
            }

            _cancellationTokenSource?.Dispose();
        }

        private void OnComplete()
        {
            _onComplete?.Invoke();
        }

        private bool _isDisposed;
        public void Dispose()
        {
            if (!this ||_isDisposed) return;
            _isDisposed = true;
            Destroy(this);
        }
    }
}