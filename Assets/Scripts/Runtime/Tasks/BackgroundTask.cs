using System.Threading;
using Cysharp.Threading.Tasks;

namespace Obert.Common.Runtime.Tasks
{
    public abstract class BackgroundTask : IBackgroundTask
    {
        public abstract UniTask Execute(CancellationToken cancellationTokenSource = default);
    }
}