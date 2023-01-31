using System;
using UnityEngine.Events;

namespace Obert.Common.Runtime.Extensions
{
    [Serializable]
    public sealed class DisposableUnityEventHook : DisposableUnityEventHookBase
    {
        private readonly UnityAction _action;

        public DisposableUnityEventHook(UnityEvent unityEvent, UnityAction action) : base(() =>
            unityEvent.RemoveListener(action))
        {
            unityEvent.AddListener(action);
        }
    }

    [Serializable]
    public sealed class DisposableUnityEventHook<T> : DisposableUnityEventHookBase
    {
        private readonly UnityAction _action;
        private readonly Action _disposeAction;

        public DisposableUnityEventHook(UnityEvent<T> unityEvent, UnityAction<T> action) : base(() =>
            unityEvent.RemoveListener(action))
        {
            unityEvent.AddListener(action);
        }

        public void Dispose()
        {
            _disposeAction?.Invoke();
        }
    }
}