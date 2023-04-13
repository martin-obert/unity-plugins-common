using System;
using UnityEngine;

namespace Obert.Common.Runtime.Tasks
{
    public sealed class TaskSchedulerFacade : MonoBehaviour
    {
        private Action _dispose;
        public static ITaskScheduler Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = new TaskScheduler(() => gameObject.AddComponent<BackgroundTaskRunner>());
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _dispose = () => { Instance = null; };
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            _dispose?.Invoke();
        }
    }
}