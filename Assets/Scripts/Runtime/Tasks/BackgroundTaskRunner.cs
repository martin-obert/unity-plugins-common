using System;
using System.Collections;
using System.Threading;
using Obert.Common.Runtime.Extensions;
using UnityEngine;

namespace Obert.Common.Runtime.Tasks
{
    public class BackgroundTaskRunner : MonoBehaviour
    {
        private CancellationTokenSource _cancellationTokenSource;

        private Coroutine _coroutine; 

        public BackgroundTask[] Tasks { get; private set; }

        private Action _onComplete;

        public void SetTasks(BackgroundTask[] tasks, Action onComplete, CancellationToken cancellationToken = default)
        {
            tasks.ThrowIfEmptyOrNull();
            _onComplete = onComplete;
            Tasks = tasks;
            if (cancellationToken != default)
            {
                _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            }
        }

        private void Start()
        {
            Tasks.ThrowIfEmptyOrNull();

            _cancellationTokenSource ??= new CancellationTokenSource();

            _coroutine = StartCoroutine(TaskCoroutine());
        }

        private IEnumerator TaskCoroutine()
        {
            foreach (var task in Tasks)
            {
                if(_cancellationTokenSource.IsCancellationRequested) break;
                
                var taskLogic = task.Execute(_cancellationTokenSource.Token);
                
                yield return taskLogic;
            }

            _coroutine = null;
            
            Destroy(this);
            OnComplete();
        }

        private void OnDestroy()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
            }
            
            if(_coroutine != null)
                StopCoroutine(_coroutine);
        }

        protected virtual void OnComplete()
        {
            _onComplete?.Invoke();
        }
    }
}