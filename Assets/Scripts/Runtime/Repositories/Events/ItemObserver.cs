using System;

namespace Obert.Common.Runtime.Repositories.Events
{
    public sealed class ItemObserver<TData> : IObserver<TData>
    {
        public Action<TData> Next { get; set; }
        private Action Completed { get; set; }
        private Action<Exception> Error { get; set; }

        public void OnCompleted()
        {
            Completed?.Invoke();
        }

        public void OnError(Exception error)
        {
            Error?.Invoke(error);
        }

        public void OnNext(TData value)
        {
            Next?.Invoke(value);
        }
    }
}