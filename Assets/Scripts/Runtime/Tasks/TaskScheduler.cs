using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Obert.Common.Runtime.Extensions;

namespace Obert.Common.Runtime.Tasks
{
    public sealed class TaskScheduler : ITaskScheduler
    {
        private readonly Func<string, IBackgroundTask[], CancellationToken, IBackgroundTaskRunner> _factory;

        public TaskScheduler(Func<string, IBackgroundTask[], CancellationToken, IBackgroundTaskRunner> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IBackgroundTaskRunner RunTasks(string id, IBackgroundTask[] tasks, CancellationToken token)
        {
            return RunTasks(id, token, tasks);
        }


        public IBackgroundTaskRunner RunTask(string id, Action<IBackgroundTask[]> onComplete, CancellationToken token,
            IBackgroundTask task)
        {
            return RunTasks(id, onComplete, token, task);
        }

        public IBackgroundTaskRunner RunTasks<T>(string id, CancellationToken token, params T[] tasks)
            where T : IBackgroundTask
        {
            return RunTasks(id, null, token, tasks);
        }

        public IBackgroundTaskRunner RunTasks<T>(string id, Action<IBackgroundTask[]> onComplete, CancellationToken token,
            params T[] tasks)
            where T : IBackgroundTask
        {
            tasks.ThrowIfEmptyOrNull();
            
            var backgroundTasks = tasks.Cast<IBackgroundTask>().ToArray();
            
            var runner = _factory(id, backgroundTasks, token);
            
            if (onComplete != null)
                runner.Complete += (_, args) =>
                {
                    foreach (var backgroundTask in args)
                    {
                        _runningTasks.Remove(backgroundTask);
                        OnRunningTasks();
                    }
                    onComplete(args);
                };

            foreach (var backgroundTask in tasks)
            {
                _runningTasks.Add(backgroundTask);
            }
            OnRunningTasks();
            
            return runner;
        }

        private readonly IList<IBackgroundTask> _runningTasks = new List<IBackgroundTask>();
        
        public event EventHandler<IEnumerable<IBackgroundTask>> RunningTasksQueue;

        private void OnRunningTasks()
        {
            RunningTasksQueue?.Invoke(this, _runningTasks);
        }

        public void Dispose()
        {
            _runningTasks?.Clear();
        }
    }
}