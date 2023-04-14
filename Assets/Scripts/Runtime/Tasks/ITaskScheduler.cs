using System;
using System.Threading;

namespace Obert.Common.Runtime.Tasks
{
    /// <summary>
    /// Scheduler, that will run tasks in parallel
    /// </summary>
    public interface ITaskScheduler
    {
        /// <summary>
        /// Run single task on new task runner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="onComplete">On complete is called regardless if there was error or not</param>
        /// <param name="token">Pass for cancellation reasons</param>
        /// <param name="task">Task to be run</param>
        /// <returns></returns>
        IBackgroundTaskRunner RunTask(string id, Action<IBackgroundTask[]> onComplete, CancellationToken token, IBackgroundTask task);

        /// <summary>
        /// Run multiple tasks on new task runner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token">Pass for cancellation reasons</param>
        /// <param name="tasks">Tasks that will be run in parallel</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IBackgroundTaskRunner RunTasks<T>(string id, CancellationToken token, params T[] tasks) where T : IBackgroundTask;

        /// <summary>
        /// Run multiple tasks on new task runner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="onComplete">On complete is called regardless if there was error or not</param>
        /// <param name="token">Pass for cancellation reasons</param>
        /// <param name="tasks">Tasks that will be run in parallel</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IBackgroundTaskRunner RunTasks<T>(string id, Action<IBackgroundTask[]> onComplete, CancellationToken token, params T[] tasks) where T : IBackgroundTask;
    }
}