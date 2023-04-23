using System;
using System.Threading;
using UnityEngine;

namespace Obert.Common.Runtime.Tasks
{
    public sealed class TaskSchedulerFacade : MonoBehaviour
    {
        private Action _dispose;
        public static ITaskScheduler Instance { get; private set; }
        private CancellationTokenSource tokenSource;

        private void Awake()
        {
            if (Instance == null)
            {
                tokenSource = new CancellationTokenSource();
                Instance = new TaskScheduler();
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _dispose = () =>
            {
                if (tokenSource is { IsCancellationRequested: true })
                {
                    tokenSource.Cancel();
                }
                tokenSource?.Dispose();
                Instance = null;
            };
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            _dispose?.Invoke();
        }
    }
}