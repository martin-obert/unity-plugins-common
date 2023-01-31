using System;

namespace Obert.Common.Runtime.Extensions
{
    [Serializable]
    public abstract class DisposableUnityEventHookBase : IDisposable
    {
        private readonly Action _disposeAction;

        protected DisposableUnityEventHookBase(Action disposeAction)
        {
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            _disposeAction?.Invoke();
        }
    }
}