using System;
using System.Collections;
using System.Threading;

namespace Obert.Common.Runtime.Tasks
{
    public interface IBackgroundTask<TResult> : IBackgroundTask
    {
        new Action<TResult> Success { get; set; }
    }

    public interface IBackgroundTask
    {
        Action Complete { get; set; }
        Action Success { get; set; }
        Action<Exception> Error { get; set; }

        IEnumerator Execute(CancellationToken cancellationTokenSource = default);
    }
}