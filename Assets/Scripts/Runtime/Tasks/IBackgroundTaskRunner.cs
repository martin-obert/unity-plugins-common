using System;

namespace Obert.Common.Runtime.Tasks
{
    /// <summary>
    /// Runner that will automatically execute all tasks in parallel
    /// </summary>
    public interface IBackgroundTaskRunner : IDisposable
    {
        /// <summary>
        /// ID of a task for back reference or debugging
        /// </summary>
        string ID { get; }
        
        /// <summary>
        /// Event hook for tasks completion. This will be raised everytime, regardless on tasks success/failiure
        /// </summary>
        event EventHandler<IBackgroundTask[]> Complete;
    }
}