using System;
using UnityEngine.Events;

namespace Obert.Common.Runtime.Extensions
{
    [Serializable]
    public sealed class DisposableUnityEventHook : DisposableUnityEventHookBase
    {
        public DisposableUnityEventHook(UnityEvent unityEvent, UnityAction action) : base(() =>
            unityEvent.RemoveListener(action))
        {
            unityEvent.AddListener(action);
        }
    }

    [Serializable]
    public sealed class DisposableUnityEventHook<T> : DisposableUnityEventHookBase
    {
        public DisposableUnityEventHook(UnityEvent<T> unityEvent, UnityAction<T> action) : base(() =>
            unityEvent.RemoveListener(action))
        {
            unityEvent.AddListener(action);
        }
    }
}