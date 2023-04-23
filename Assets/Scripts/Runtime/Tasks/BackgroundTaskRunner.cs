using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Obert.Common.Runtime.Extensions;
using Obert.Common.Runtime.Logging;
using Logger = Obert.Common.Runtime.Logging.Logger;

namespace Obert.Common.Runtime.Tasks
{
    public sealed class BackgroundTaskRunner : IBackgroundTaskRunner
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly IBackgroundTask[] _tasks;

        private readonly ILogger<IBackgroundTaskRunner> _logger;

        private bool _isDisposed;

        public event EventHandler<IBackgroundTask[]> Complete;

        public string ID { get; }

        public BackgroundTaskRunner(string id, IBackgroundTask[] tasks, CancellationToken cancellationToken = default)
        {
            tasks.ThrowIfEmptyOrNull();
            _logger = Logger.CreateFor<IBackgroundTaskRunner>();

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _tasks = tasks;
            ID = id;
        }

        public IBackgroundTaskRunner Execute()
        {
            _tasks.ThrowIfEmptyOrNull();

            UniTask.Void(async t =>
                {
                    await UniTask.WhenAll(_tasks.Select(async x =>
                    {
                        try
                        {
                            await x.Execute(t);
                            _logger.Log($"{ID} - task completed - {x.ID}");
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"{ID} - task failed - {x.ID} - exception -> {e}");
                        }
                    }));
                    UniTask.ReturnToMainThread(t);
                    Dispose();
                },
                _cancellationTokenSource.Token);

            return this;
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource?.Cancel();
            }

            _cancellationTokenSource?.Dispose();
            OnComplete();
        }

        private void OnComplete()
        {
            _logger.Log($"{ID} - runner completed");
            Complete?.Invoke(this, _tasks);
        }
    }
}