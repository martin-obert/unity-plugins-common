using System.Threading;
using Cysharp.Threading.Tasks;

namespace Obert.Common.Runtime.Tasks
{
    public interface IBackgroundTask
    {
        UniTask Execute(CancellationToken cancellationToken = default);
        
        string ID { get; }
    }
}