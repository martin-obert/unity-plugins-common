using System.Threading;

namespace Obert.Common.Runtime.Tasks
{
    public interface ITaskScheduler
    {
        ITaskScheduler RunTasks(params BackgroundTask[] tasks);
        ITaskScheduler RunTasks(BackgroundTask[] tasks, CancellationToken token);
    }
}