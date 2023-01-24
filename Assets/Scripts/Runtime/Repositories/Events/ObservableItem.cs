using System;
using System.Collections.Generic;

namespace Obert.Common.Runtime.Repositories.Events
{
    internal sealed class ObservableItem<TData> : IObservable<TData>, IDisposable, IObserver<TData>
    {
        private class Hook : IDisposable
        {
            public Hook(IObserver<TData> observer)
            {
                Observer = observer;
            }

            public IObserver<TData> Observer { get; }

            public Action Unsubscribe { get; set; }

            public void Dispose()
            {
                Unsubscribe?.Invoke();
            }
        }

        private readonly List<Hook> _subscribers = new();

        public IDisposable Subscribe(IObserver<TData> observer)
        {
            var disposable = new Hook(observer);

            disposable.Unsubscribe = () => Unsubscribe(disposable);

            _subscribers.Add(disposable);

            return disposable;
        }

        private void Unsubscribe(Hook hook)
        {
            if (hook == null) return;

            if (!_subscribers.Remove(hook)) return;
            hook.Dispose();
        }

        public void Dispose()
        {
            for (var index = _subscribers.Count - 1; index >= 0; index--)
            {
                var subscriber = _subscribers[index];
                subscriber?.Dispose();
            }
        }

        public void OnCompleted()
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber?.Observer?.OnCompleted();
            }
        }
        
        public void OnError(Exception error)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber?.Observer?.OnError(error);
            }
        }

        public void OnNext(TData value)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber?.Observer?.OnNext(value);
            }
        }
    }
}