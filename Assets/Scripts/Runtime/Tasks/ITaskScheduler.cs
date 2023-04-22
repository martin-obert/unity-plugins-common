using System;
using System.Collections.Generic;
using System.Threading;

namespace Obert.Common.Runtime.Tasks
{
    /// <summary>
    /// Scheduler, that will run tasks in parallel
    /// </summary>
    public interface ITaskScheduler : IDisposable
    {
        /// <summary>
        /// Run single task on new task runner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="onComplete">On complete is called regardless if there was error or not</param>
        /// <param name="token">Pass for cancellation reasons</param>
        /// <param name="task">Task to be run</param>
        /// <returns></returns>
        IBackgroundTaskRunner RunTask(string id, Action<IBackgroundTask[]> onComplete, CancellationToken token,
            IBackgroundTask task);

        /// <summary>
        /// Run multiple tasks on new task runner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token">Pass for cancellation reasons</param>
        /// <param name="tasks">Tasks that will be run in parallel</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IBackgroundTaskRunner RunTasks<T>(string id, CancellationToken token, params T[] tasks)
            where T : IBackgroundTask;

        /// <summary>
        /// Run multiple tasks on new task runner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="onComplete">On complete is called regardless if there was error or not</param>
        /// <param name="token">Pass for cancellation reasons</param>
        /// <param name="tasks">Tasks that will be run in parallel</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IBackgroundTaskRunner RunTasks<T>(string id, Action<IBackgroundTask[]> onComplete, CancellationToken token,
            params T[] tasks) where T : IBackgroundTask;

        /// <summary>
        /// Represents current state of a Background Tasks queue. <see cref="EventArgs"/> contains all active <see cref="IBackgroundTask"/>
        /// Emits each time new background tasks are running or completed.
        /// </summary>
        /// <example>
        /// If we schedule 10 tasks, each time one of the tasks completes the observable emits current state of queue. So it returns only running tasks.
        /// </example>
        event EventHandler<IEnumerable<IBackgroundTask>> RunningTasksQueue;
    }
}