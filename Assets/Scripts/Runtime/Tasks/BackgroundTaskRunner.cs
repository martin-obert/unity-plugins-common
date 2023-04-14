using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Obert.Common.Runtime.Extensions;
using Obert.Common.Runtime.Logging;
using UnityEngine;
using Logger = Obert.Common.Runtime.Logging.Logger;

namespace Obert.Common.Runtime.Tasks
{
    public sealed class BackgroundTaskRunner : MonoBehaviour
    {
        public Controller InstanceController { get; private set; }
        private bool _isDisposed;

        public sealed class Controller : IBackgroundTaskRunner
        {
            private readonly CancellationTokenSource _cancellationTokenSource;

            private readonly IBackgroundTask[] _tasks;

            private readonly ILogger<IBackgroundTaskRunner> _logger;

            private bool _isDisposed;

            public event EventHandler<IBackgroundTask[]> Complete;

            public string ID { get; }

            public Controller(string id, IBackgroundTask[] tasks, CancellationToken cancellationToken = default)
            {
                tasks.ThrowIfEmptyOrNull();
                _logger = Logger.CreateFor<IBackgroundTaskRunner>();

                _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                _tasks = tasks;
                ID = id;
            }

            public void Initialize()
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

        public void SetController(Controller controller)
        {
            if (InstanceController != null) throw new Exception("Controller is already set");
            InstanceController = controller ?? throw new ArgumentNullException(nameof(controller));
            InstanceController.Complete += InstanceControllerOnComplete;
        }

        private void InstanceControllerOnComplete(object sender, IBackgroundTask[] e)
        {
            if (this)
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            if (InstanceController == null) throw new ArgumentNullException(nameof(InstanceController));

            InstanceController.Initialize();
        }

        private void OnDestroy()
        {
            if (!this || _isDisposed) return;
            _isDisposed = true;

            if (InstanceController == null) return;
            InstanceController.Complete -= InstanceControllerOnComplete;
            InstanceController.Dispose();
        }
    }
}