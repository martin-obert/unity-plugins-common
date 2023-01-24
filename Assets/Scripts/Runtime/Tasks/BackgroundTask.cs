using System;
using System.Collections;
using System.Threading;

namespace Obert.Common.Runtime.Tasks
{
    public abstract class BackgroundTask<TResult> : BackgroundTask, IBackgroundTask<TResult>
    {
        public new Action<TResult> Success { get; set; }
    }

    public abstract class BackgroundTask : IBackgroundTask
    {
        public Action  Complete { get; set; }
        public Action  Success { get; set; }
        public Action<Exception> Error { get; set; }

        public abstract IEnumerator Execute(CancellationToken cancellationTokenSource = default);

        protected virtual void OnComplete()
        {
            Complete?.Invoke();
        }

        protected virtual void OnError(Exception e)
        {
            Error?.Invoke(e);
        }

        protected virtual void OnSuccess()
        {
            Success?.Invoke();
        }
    }
}