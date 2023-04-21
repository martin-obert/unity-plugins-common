using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Obert.Common.Runtime.Tasks
{
    public sealed class BasicBackgroundTask : IBackgroundTask
    {
        public string ID { get; }
        private readonly Func<CancellationToken, UniTask> _method;

        public BasicBackgroundTask(Func<CancellationToken, UniTask> method, string id = null)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
            ID = id ?? Guid.NewGuid().ToString();
        }


        public UniTask Execute(CancellationToken cancellationToken = default) => _method(cancellationToken);
    }
}