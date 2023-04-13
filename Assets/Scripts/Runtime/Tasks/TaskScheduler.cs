using System;
using System.Linq;
using System.Threading;
using Obert.Common.Runtime.Extensions;

namespace Obert.Common.Runtime.Tasks
{
    public sealed class TaskScheduler : ITaskScheduler
    {
        private readonly Func<IBackgroundTaskRunner> _factory;

        public TaskScheduler(Func<IBackgroundTaskRunner> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        
        public IBackgroundTaskRunner RunTasks(IBackgroundTask[] tasks, CancellationToken token)
        {
            return RunTasks(token, tasks);
        }


        public IBackgroundTaskRunner RunTask(Action onComplete, CancellationToken token, IBackgroundTask task)
        {
            return RunTasks(onComplete, token, task);
        }

        public IBackgroundTaskRunner RunTasks<T>(CancellationToken token, params T[] tasks) where T : IBackgroundTask
        {
            return RunTasks(null, token, tasks);
        }

        public IBackgroundTaskRunner RunTasks<T>(Action onComplete, CancellationToken token, params T[] tasks)
            where T : IBackgroundTask
        {
            tasks.ThrowIfEmptyOrNull();
            var backgroundTasks = tasks.Cast<IBackgroundTask>().ToArray();
            var runner = _factory();
            runner.SetTasks(backgroundTasks, onComplete, token);

            return runner;
        }
    }
}