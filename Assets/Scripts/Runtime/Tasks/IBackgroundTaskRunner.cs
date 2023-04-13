using System;
using System.Threading;

namespace Obert.Common.Runtime.Tasks
{
    public interface IBackgroundTaskRunner : IDisposable
    {
        void SetTasks(IBackgroundTask[] backgroundTasks, Action onComplete, CancellationToken token);
    }
}