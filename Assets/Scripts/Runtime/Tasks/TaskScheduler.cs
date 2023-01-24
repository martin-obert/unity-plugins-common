using System;
using System.Threading;
using Obert.Common.Runtime.Extensions;
using UnityEngine;

namespace Obert.Common.Runtime.Tasks
{
    public class TaskScheduler : MonoBehaviour, ITaskScheduler
    {
        public static TaskScheduler Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (Instance != this)
                {
                    Destroy(gameObject);
                    return;
                }
            }

            DontDestroyOnLoad(gameObject);
        }

        public ITaskScheduler RunTasks(params BackgroundTask[] tasks) => RunTasks(tasks, CancellationToken.None);
        public ITaskScheduler RunTask(BackgroundTask task, CancellationToken cancellationToken = default) => RunTask(task, null, cancellationToken);
        public ITaskScheduler RunTask(BackgroundTask task, Action onComplete, CancellationToken cancellationToken = default) => RunTasks(new[] { task }, onComplete, cancellationToken);

        public ITaskScheduler RunTasks(BackgroundTask[] tasks, CancellationToken token)
        {
            tasks.ThrowIfEmptyOrNull();

            gameObject.AddComponent<BackgroundTaskRunner>().SetTasks(tasks, null, token);

            return this;
        }
        public ITaskScheduler RunTasks(BackgroundTask[] tasks, Action onComplete, CancellationToken token)
        {
            tasks.ThrowIfEmptyOrNull();

            gameObject.AddComponent<BackgroundTaskRunner>().SetTasks(tasks, onComplete, token);

            return this;
        }
    }
}