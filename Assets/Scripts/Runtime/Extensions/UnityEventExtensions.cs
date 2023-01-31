using System;
using UnityEngine.Events;

namespace Obert.Common.Runtime.Extensions
{
    public static class UnityEventExtensions
    {
        public static IDisposable Subscribe(this UnityEvent source, UnityAction action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new DisposableUnityEventHook(source, action);
        }
        
        public static IDisposable Subscribe<T>(this UnityEvent<T> source, UnityAction<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new DisposableUnityEventHook<T>(source, action);
        }
    }
}